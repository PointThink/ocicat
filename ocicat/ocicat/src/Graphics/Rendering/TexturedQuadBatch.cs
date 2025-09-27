using ocicat.Graphics.Rendering;
using OpenTK.Mathematics;
using ocicat.Graphics;
using System.Transactions;

namespace ocicat;

public class TexturedQuadBatch
{
    private VertexArray _vertexArray;
    private VertexBuffer _vertexBuffer;
    private IndexBuffer _indexBuffer;

    private float[] _verticies;
    private uint[] _indices;
    private Texture[] _textures;

    private uint _vertexOffset = 0;
    private uint _indexOffset = 0;
    private uint _indexStride = 0;

    private uint _textureOffset = 1;

    public TexturedQuadBatch(Renderer renderer, int quadCount)
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

        _textures = new Texture[32];
    }


    public void AddQuad(Vector2 position, Vector2 size, Color color, Texture texture, float rotation = 0)
    {
        Matrix4 transform = Matrix4.CreateTranslation(-position.X - size.X / 2, -position.Y - size.Y / 2, 0) *
            Matrix4.CreateRotationZ(Single.DegreesToRadians(rotation)) *
            Matrix4.CreateTranslation(position.X + size.X / 2, position.Y + size.Y / 2, 0);

        float textureSlot = FindTextureSlotForTexture(texture);

        WriteVertex(position, color, new Vector2(0, 1), textureSlot, transform);
        WriteVertex(position + new Vector2(size.X, 0), color, new Vector2(1, 1), textureSlot, transform);
        WriteVertex(position + new Vector2(size.X, size.Y), color, new Vector2(1, 0), textureSlot, transform);
        WriteVertex(position + new Vector2(0, size.Y), color, new Vector2(0, 0), textureSlot, transform);

        _indices[_indexOffset] = _indexStride;
        _indices[_indexOffset + 1] = _indexStride + 1;
        _indices[_indexOffset + 2] = _indexStride + 2;
        _indices[_indexOffset + 3] = _indexStride + 2;
        _indices[_indexOffset + 4] = _indexStride + 3;
        _indices[_indexOffset + 5] = _indexStride;

        _indexStride += 4;
        _indexOffset += 6;
    }

    private void WriteVertex(Vector2 position, Color color, Vector2 UVs, float textureSlot, Matrix4 transform)
    {
        Vector3 translatedPos = Vector3.TransformPosition(new Vector3(position.X, position.Y, 0), transform);

        _verticies[_vertexOffset] = translatedPos.X;
        _verticies[_vertexOffset + 1] = translatedPos.Y;
        _verticies[_vertexOffset + 2] = color.R;
        _verticies[_vertexOffset + 3] = color.G;
        _verticies[_vertexOffset + 4] = color.B;
        _verticies[_vertexOffset + 5] = color.A;
        _verticies[_vertexOffset + 6] = UVs.X;
        _verticies[_vertexOffset + 7] = UVs.Y;
        _verticies[_vertexOffset + 8] = textureSlot;

        _vertexOffset += 9;
    }

    public void Render(Renderer renderer)
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
        renderer.Primitives.BatchedQuadShader.UniformMat4("projection", renderer.CameraProjection);
        renderer.Primitives.BatchedQuadShader.UniformMat4("transform", renderer.CameraView);

        renderer.RenderCommands.DrawIndexed(_vertexArray);

        _vertexOffset = 0;
        _indexOffset = 0;
        _indexStride = 0;
        _textureOffset = 0;

        for (int i = 0; i < _textures.Length; i++)
            _textures[i] = null;
    }

    // returns 0 - not found
    private int FindTextureInBatch(Texture texture)
    {
        for (int i = 0; i < _textures.Length; i++)
        {
            if (texture == _textures[i])
                return i;
            else if (texture == null)
                return 0;
        }

        return -1;
    }

    public uint FindTextureSlotForTexture(Texture texture)
    {
        int textureSlot = FindTextureInBatch(texture);

        if (textureSlot == -1)
        {
            _textures[_textureOffset] = texture;
            _textureOffset++;

            return _textureOffset - 1;
        }

        return (uint) textureSlot;
    }
}