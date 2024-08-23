using OpenTK.Graphics.OpenGL4;

namespace ocicat.Graphics.Rendering.OpenGl;

public class Texture : Rendering.Texture
{
	private int _handle;
	private int _width;
	private int _height;
	
	public Texture(byte[] imageData, int width, int height, TextureFilter textureFilter, int colorChannels)
	{
		_handle = GL.GenTexture();

		_width = width;
		_height = height;

		PixelFormat format;
		
		switch (colorChannels)
		{
			case 1:
				format = PixelFormat.Red;
				break;
			case 3:
				format = PixelFormat.Rgb;
				break;
			case 4:
				format = PixelFormat.Rgba;
				break;
			default:
				throw new ArgumentException($"Invalid channel count: {colorChannels}");
		}
		
		GL.BindTexture(TextureTarget.Texture2D, _handle);
		
		if (textureFilter == TextureFilter.Linear)
		{
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter,
				(int)TextureMinFilter.Linear);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter,
				(int)TextureMagFilter.Linear);
		}
		else
		{
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter,
				(int)TextureMinFilter.Nearest);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter,
				(int)TextureMagFilter.Nearest);
		}
		
		GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int) TextureWrapMode.Repeat);
		GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapR, (int) TextureWrapMode.Repeat);
		
		GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba8, width, height, 0, format, PixelType.UnsignedByte, imageData);
		GL.BindTexture(TextureTarget.Texture2D, 0);
		
		Logging.Log(LogLevel.Ocicat, $"Created OpenGL Texture:\n\tHandle: {_handle}\n\tWidth: {width}\n\tWidth: {height}\n\tFilter: {Enum.GetName(typeof(TextureFilter), textureFilter)}\n\tChannels: {colorChannels}");
	}

	~Texture()
	{
		GL.DeleteTexture(_handle);
		Logging.Log(LogLevel.Ocicat, $"Disposed OpenGL texture {_handle}");
	}

	public override void Bind(uint slot)
	{
		uint textureUnit = ((uint)TextureUnit.Texture0) + slot;
		
		GL.ActiveTexture((TextureUnit) textureUnit);
		GL.BindTexture(TextureTarget.Texture2D, _handle);
	}

	public override void Unbind()
	{
		GL.BindTexture(TextureTarget.Texture2D, 0);
	}

	public override int GetWidth()
	{
		return _width;
	}

	public override int GetHeight()
	{
		return _height;
	}

	public override int GetTextureID()
	{
		return _handle;
	}
}