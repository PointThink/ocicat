namespace ocicat.Graphics;

public class Color
{
	public float R, G, B, A;
	
	private Color(float r, float g, float b, float a)
	{
		R = r;
		G = g;
		B = b;
		A = a;
	}

	public static Color CreateFloat(float r, float g, float b, float a)
	{
		return new Color(r, g, b, a);
	}

	public static Color CreateRGBA8(byte r, byte g, byte b, byte a)
	{
		return new Color(
			r / 255f,
			g / 255f,
			b / 255f,
			a / 255f
		);
	}
}