namespace ocicat.Graphics.Rendering;

public class Mesh
{
	private VertexBuffer _vertexBuffer;
	private IndexBuffer _indexBuffer;
	public VertexArray VertexArray { get; private set; }
	
	public Mesh(Renderer renderer, float[] vertecies, uint[] indices, BufferLayout layout)
	{
		_vertexBuffer = VertexBuffer.Create(renderer, vertecies);
		_vertexBuffer.Layout = layout;
		_indexBuffer = IndexBuffer.Create(renderer, indices);
		
		VertexArray = VertexArray.Create(renderer);
		VertexArray.SetVertexBuffer(_vertexBuffer);
		VertexArray.SetIndexBuffer(_indexBuffer);
	}

	public static Mesh GenCircleMesh(Renderer renderer, int pointCount)
	{
		List<float> vertecies = new List<float>();
		
		// Generate circle mesh
		for (int i = 0; i < pointCount; i++)
		{
			double degrees = 360f / pointCount * i;

			Vector2 vertex1 = Vector2.FromDegrees((float) degrees);
			Vector2 vertex2 = Vector2.FromDegrees((float) degrees + 360f / pointCount);

			vertecies.Add(0);
			vertecies.Add(0);
			vertecies.Add(vertex1.X);
			vertecies.Add(vertex1.Y);
			vertecies.Add(vertex2.X);
			vertecies.Add(vertex2.Y);
		}
		
		uint[] indicies = new uint[pointCount * 3];

		for (uint i = 0; i < pointCount * 3; i++)
			indicies[i] = i;

		Mesh mesh = new Mesh(renderer, vertecies.ToArray(), indicies, new BufferLayout(
			[new BufferElement("position", ShaderDataType.Float2)]
		));

		return mesh;
	}
}