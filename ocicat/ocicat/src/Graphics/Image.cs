using StbImageSharp;

namespace ocicat.Graphics;

public class Image
{
	public byte[] Data { get; private set; }
	
	public int Width { get; private set; }
	public int Height { get; private set; }
	
	public Image(string path)
	{
		ImageResult image = ImageResult.FromStream(File.OpenRead(path), ColorComponents.RedGreenBlueAlpha);

		Width = image.Width;
		Height = image.Height;
		Data = image.Data;
	}
}