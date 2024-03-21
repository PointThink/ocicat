namespace ocicat.Graphics.Rendering;

public abstract class VertexBuffer
{
	public BufferLayout? Layout = null;
	
	protected VertexBuffer() {}

	public abstract void Bind();
	public abstract void Unbind();
	
	public static VertexBuffer? Create(Renderer renderer, float[] data)
	{
		switch (renderer.RenderingApi)
		{
			case RenderingApi.OpenGl:
				return new OpenGl.VertexBuffer(data);
				break;
		}

		return null;
	}
}

public abstract class IndexBuffer
{
	protected IndexBuffer() {}

	public abstract void Bind();
	public abstract void Unbind();

	public abstract uint GetIndexCount();
	
	public static IndexBuffer? Create(Renderer renderer, uint[] data)
	{
		switch (renderer.RenderingApi)
		{
			case RenderingApi.OpenGl:
				return new OpenGl.IndexBuffer(data);
				break;
		}

		return null;
	}
}