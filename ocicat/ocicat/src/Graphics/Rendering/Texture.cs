using StbImageSharp;

namespace ocicat.Graphics.Rendering;

public enum TextureFilter
{
	Linear, Nearest		
}

/// <summary>Image on the GPU.</summary>
/// <remarks>Constructor is protected. Use the Create method</remarks>
public abstract class Texture
{
	public abstract void Bind(uint slot);
	public abstract void Unbind();

	public abstract int GetWidth();
	public abstract int GetHeight();

	public abstract int GetTextureID();

	public static Texture Create(Renderer renderer, byte[] imageData, int width, int height, TextureFilter textureFilter = TextureFilter.Nearest, int colorChannels = 4)
	{
		switch (renderer.RenderingApi)
		{
			case RenderingApi.OpenGl:
				return new OpenGl.Texture(imageData, width, height, textureFilter, colorChannels);
		}

		throw new ArgumentException("Invalid RenderingApi");
	}

	public static Texture Create(Renderer renderer, string filePath, TextureFilter textureFilter = TextureFilter.Nearest)
	{
		Image image = new Image(filePath);
		return Texture.Create(renderer, image.Data, image.Width, image.Height, textureFilter);
	}
}