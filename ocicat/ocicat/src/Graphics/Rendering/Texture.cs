using StbImageSharp;

namespace ocicat.Graphics.Rendering;

public enum TextureFilter
{
	Linear, Nearest		
}

public abstract class Texture
{
	public abstract void Bind(uint slot);
	public abstract void Unbind();
	
	public abstract int GetWidth();
	public abstract int GetHeight(uint slot);
	
	public static Texture? Create(Renderer renderer, byte[] imageData, int width, int height, TextureFilter textureFilter = TextureFilter.Linear, int colorChannels = 4)
	{
		switch (renderer.RenderingApi)
		{
			case RenderingApi.OpenGl:
				return new OpenGl.Texture(imageData, width, height, textureFilter, colorChannels);
		}

		return null;
	}

	public static Texture? Create(Renderer renderer, string filePath, TextureFilter textureFilter = TextureFilter.Linear)
	{
		ImageResult image = ImageResult.FromStream(File.OpenRead(filePath), ColorComponents.RedGreenBlueAlpha);
        
		return Create(renderer, image.Data, image.Width, image.Height, textureFilter);
	}
}