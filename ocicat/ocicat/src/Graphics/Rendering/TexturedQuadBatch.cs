using ocicat.Graphics.Rendering;
using OpenTK.Mathematics;
using ocicat.Graphics;

namespace ocicat;

public struct BatchedVertex
{
    public Vector2 Position;
    public Color Color;
    public Vector2 TextureCoords;
}

public class TexturedBatch
{
    private VertexArray _vertexArray;
    private VertexBuffer _vertexBuffer;
    private IndexBuffer _indexBuffer;

    private float[] _verticies;
    private uint[] _indices;
    private Texture?[] _textures;

    private uint _vertexOffset = 0;
    private uint _indexOffset = 0;
    private uint _indexStride = 0;

    private uint _textureOffset = 1;

    public TexturedBatch(Renderer renderer, int quadCount)
    {
        _vertexBuffer = VertexBuffer.Create(renderer, quadCount * 4 * 9);
        _vertexBuffer.Layout = new BufferLayout([
            new BufferElement("position", ShaderDataType.Float2),
            new BufferElement("color", ShaderDataType.Float4),
            new BufferElement("uvs", ShaderDataType.Float2),
            new BufferElement("textureID", ShaderDataType.Float)
        ]);

        _indexBuffer = IndexBuffer.Create(renderer, quadCount * 6);

        _vertexArray = VertexArray.Create(renderer);
        _vertexArray.SetVertexBuffer(_vertexBuffer);
        _vertexArray.SetIndexBuffer(_indexBuffer);

        _verticies = new float[quadCount * 4 * 9];
        _indices = new uint[quadCount * 6];

        _textures = new Texture?[32];
        _textures[0] = renderer.Primitives.WhiteTexture;
    }

    public void AddQuad(BatchedVertex[] quad, Texture? texture = null)
    {
        uint textureSlot = FindTextureSlotForTexture(texture);

        foreach (BatchedVertex vert in quad)
        {
            _verticies[_vertexOffset] = vert.Position.X;
            _verticies[_vertexOffset + 1] = vert.Position.Y;
            _verticies[_vertexOffset + 2] = vert.Color.R;
            _verticies[_vertexOffset + 3] = vert.Color.G;
            _verticies[_vertexOffset + 4] = vert.Color.B;
            _verticies[_vertexOffset + 5] = vert.Color.A;
            _verticies[_vertexOffset + 6] = vert.TextureCoords.X;
            _verticies[_vertexOffset + 7] = vert.TextureCoords.Y;
            _verticies[_vertexOffset + 8] = textureSlot;

            _vertexOffset += 9;
        }

        _indices[_indexOffset] = _indexStride;
        _indices[_indexOffset + 1] = _indexStride + 1;
        _indices[_indexOffset + 2] = _indexStride + 2;
        _indices[_indexOffset + 3] = _indexStride + 2;
        _indices[_indexOffset + 4] = _indexStride + 3;
        _indices[_indexOffset + 5] = _indexStride;

        _indexStride += 4;
        _indexOffset += 6;
    }

    public BatchedVertex[] CreateQuad(Vector2 position, Vector2 size, Color color)
    {
        BatchedVertex[] quad = new BatchedVertex[4];

        quad[0].Position = position;
        quad[0].Color = color;
        quad[0].TextureCoords = new Vector2(0, 0);

        quad[1].Position = position + new Vector2(size.X, 0);
        quad[1].Color = color;
        quad[1].TextureCoords = new Vector2(1, 0);

        quad[2].Position = position + new Vector2(size.X, size.Y);
        quad[2].Color = color;
        quad[2].TextureCoords = new Vector2(1, 1);

        quad[3].Position = position + new Vector2(0, size.Y);
        quad[3].Color = color;
        quad[3].TextureCoords = new Vector2(0, 1);

        return quad;
    }

    public void Render(Renderer renderer, Camera camera)
    {
        _vertexBuffer.ReplaceData(_verticies);
        _indexBuffer.ReplaceData(_indices);

        int[] textureSlots = new int[32];

        for (uint i = 0; _textures[i] != null; i++)
        {
            _textures[i].Bind(i);
            textureSlots[i] = (int)i;
        }

        renderer.Primitives.BatchedQuadShader.Use();
        renderer.Primitives.BatchedQuadShader.Uniform1iArray("textures[0]", textureSlots);
        renderer.Primitives.BatchedQuadShader.UniformMat4("projection", camera.CalculateProjection());
        renderer.Primitives.BatchedQuadShader.UniformMat4("transform", camera.CalculateView());

        renderer.RenderCommands.DrawIndexed(_vertexArray);

        _vertexOffset = 0;
        _indexOffset = 0;
        _indexStride = 0;
        _textureOffset = 0;

        for (int i = 0; i < _textures.Length; i++)
            _textures[i] = null;
    }

    // returns 0 - not found
    private uint FindTextureInBatch(Texture texture)
    {
        if (texture == null)
            return 0;

        for (uint i = 1; i < _textures.Length; i++)
        {
            if (texture == _textures[i])
                return i;
            else if (texture == null)
                return 0;
        }

        return 0;
    }

    public uint FindTextureSlotForTexture(Texture? texture)
    {
        if (texture == null)
            return 0;

        uint textureSlot = FindTextureInBatch(texture);

        if (textureSlot == 0)
        {
            _textures[_textureOffset] = texture;
            _textureOffset++;
        }

        return textureSlot;
    }
}