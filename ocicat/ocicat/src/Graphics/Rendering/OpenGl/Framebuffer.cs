using OpenTK.Graphics.OpenGL;

namespace ocicat.Graphics.Rendering.OpenGl;

public class Framebuffer : Rendering.Framebuffer
{
	private int _handle;

	private Texture _colorAttachment; 
	
	public Framebuffer(int width, int height)
	{
		_handle = GL.GenFramebuffer();
		GL.BindFramebuffer(FramebufferTarget.Framebuffer, _handle);

		_colorAttachment = new Texture([], width, height, TextureFilter.Linear, 4);
		GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, TextureTarget.Texture2D, _colorAttachment.GetTextureID(), 0);

		if (GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer) != FramebufferErrorCode.FramebufferComplete)
			Logging.Log(LogLevel.Warning, "Framebuffer broken :(");
		
		Unbind();
	}

	~Framebuffer()
	{
		GL.DeleteFramebuffer(_handle);
	}

	public override void Bind()
	{
		GL.BindFramebuffer(FramebufferTarget.Framebuffer, _handle);
	}

	public override void Unbind()
	{
		GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
	}

	public override Rendering.Texture GetTextureAttachment()
	{
		return _colorAttachment;
	}
}