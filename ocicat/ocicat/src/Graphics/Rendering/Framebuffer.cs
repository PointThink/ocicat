namespace ocicat.Graphics.Rendering;

public abstract class Framebuffer
{
	protected Framebuffer() { }
	
	public abstract void Bind();
	public abstract void Unbind();

	public abstract Texture GetTextureAttachment();
	
	public static Framebuffer Create(Renderer renderer, int width, int height)
	{
		switch (renderer.RenderingApi)
		{
			case RenderingApi.OpenGl:
				return new OpenGl.Framebuffer(width, height, renderer.Window.AASamples);
		}

		throw new ArgumentException("Invalid RenderingApi");
	}
}