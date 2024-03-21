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
}