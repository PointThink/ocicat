namespace ocicat.Graphics.Rendering;

public abstract class VertexBuffer
{
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