using StbImageSharp;

namespace ocicat.Graphics;

public class Image
{
	public byte[] Data { get; private set; }
	
	public int Width { get; private set; }
	public int Height { get; private set; }
	
	public Image(string path)
	{
		try
		{
			ImageResult image = ImageResult.FromStream(File.OpenRead(path), ColorComponents.RedGreenBlueAlpha);

			Width = image.Width;
			Height = image.Height;
			Data = image.Data;
		}
		catch (FileNotFoundException)
		{
			Logging.Log(LogLevel.Error, $"Error loading image: Cannot find file {path}");

			// Generate a 16x16 standard pink-black grid
			
			Width = 16;
			Height = 16;
			Data = new byte[16 * 16 * 4];
			uint[] u32Data = new uint[16 * 16];

			for (int i = 0; i < 16; i++)
			{
				for (int j = 0; j < 16; j++)
				{
					if ((i * 15 + j) % 2 == 0)
						u32Data[j * 16 + i] = 0xFFFF00FF;
					else
						u32Data[j * 16 + i] = 0xFF000000;
				}
			}

			Data = u32Data.SelectMany(BitConverter.GetBytes).ToArray();
		}
	}
}