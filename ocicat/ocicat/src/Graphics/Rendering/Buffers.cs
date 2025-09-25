namespace ocicat.Graphics.Rendering;

public abstract class VertexBuffer
{
	public BufferLayout? Layout = null;

	protected VertexBuffer() { }

	public abstract void Bind();
	public abstract void Unbind();

	public abstract void ReplaceData(float[] data);

	public static VertexBuffer Create(Renderer renderer, float[] data)
	{
		switch (renderer.RenderingApi)
		{
			case RenderingApi.OpenGl:
				return new OpenGl.VertexBuffer(data);
		}

		throw new ArgumentException("Invalid RenderingApi");
	}

	public static VertexBuffer Create(Renderer renderer, int sizeFloats)
	{
		switch (renderer.RenderingApi)
		{
			case RenderingApi.OpenGl:
				return new OpenGl.VertexBuffer(sizeFloats);
		}

		throw new ArgumentException("Invalid RenderingApi");
	}
}

public abstract class IndexBuffer
{
	protected IndexBuffer() { }

	public abstract uint IndexCount { get; }

	public abstract void Bind();
	public abstract void Unbind();

	public abstract void ReplaceData(uint[] data);

	public static IndexBuffer Create(Renderer renderer, uint[] data)
	{
		switch (renderer.RenderingApi)
		{
			case RenderingApi.OpenGl:
				return new OpenGl.IndexBuffer(data);
		}

		throw new ArgumentException("Invalid RenderingApi");
	}
	
	public static IndexBuffer Create(Renderer renderer, int indexCount)
	{
		switch (renderer.RenderingApi)
		{
			case RenderingApi.OpenGl:
				return new OpenGl.IndexBuffer(indexCount);
		}

		throw new ArgumentException("Invalid RenderingApi");
	}
}