namespace ocicat.Graphics;

public struct Color
{
	public static readonly Color White = CreateFloat(1, 1, 1, 1);
	public static readonly Color Red = CreateFloat(1, 0, 0, 1);
	public static readonly Color Green = CreateFloat(0, 1, 0, 1);
	public static readonly Color Blue = CreateFloat(0, 0, 1, 1);
	public static readonly Color Yellow = CreateFloat(1, 1, 0, 1);
	public static readonly Color Magenta = CreateFloat(1, 0, 1, 1);
	public static readonly Color Cyan = CreateFloat(0, 1, 1, 1);
	public static readonly Color Black = CreateFloat(0, 0, 0, 1);
	
	public float R, G, B, A;
	
	private Color(float r, float g, float b, float a)
	{
		R = r;
		G = g;
		B = b;
		A = a;
	}
	

	/// <summary>
	/// Create a color with 0 to 1 values
	/// </summary>
	public static Color CreateFloat(float r, float g, float b, float a)
	{
		return new Color(r, g, b, a);
	}

	/// <summary>
	/// Create a color with 0-255 values
	/// </summary>
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