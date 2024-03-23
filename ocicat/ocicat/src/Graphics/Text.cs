using System.Runtime.InteropServices;
using FreeTypeSharp.Native;
using ocicat.Graphics.Rendering;
using OpenTK.Mathematics;
using Vector2 = System.Numerics.Vector2;

namespace ocicat.Graphics;

public class FontGlyph
{
	public FontGlyph(Texture texture, int sizeX, int sizeY, int bearingX, int bearingY, uint advance)
	{
		Texture = texture;
		SizeX = sizeX;
		SizeY = sizeY;
		BearingX = bearingX;
		BearingY = bearingY;
		Advance = advance;
	}
	
	public readonly Texture Texture;

	public readonly int SizeX;
	public readonly int SizeY;
	public readonly int BearingX;
	public readonly int BearingY;

	public readonly uint Advance;
}

public unsafe class Font
{
	public int SpaceSize = 10;
	public int TabSize = 40;
	
	private IntPtr _freetype;
	private FT_FaceRec* _freetypeFace;
	
	private int _size;

	private Dictionary<uint, FontGlyph> _glyphs = new Dictionary<uint, FontGlyph>();
	
	public Font(Renderer renderer, string path, uint size)
	{
		FT.FT_Init_FreeType(out _freetype);
		FT.FT_New_Face(_freetype, path, 0, out IntPtr tempFace);
		FT.FT_Set_Pixel_Sizes((IntPtr)_freetypeFace, 0, size);
		_freetypeFace = (FT_FaceRec*) tempFace;
		
		// Load all glyphs
		for (uint ch = 0; ch < 128; ch++)
		{
			if (FT.FT_Load_Char((IntPtr) _freetypeFace, ch, FT.FT_LOAD_RENDER) == FT_Error.FT_Err_Ok)
			{
				Vector2i charSize = new Vector2i( (int) _freetypeFace->glyph->bitmap.width,  (int) _freetypeFace->glyph->bitmap.rows);
				Vector2i bearing = new Vector2i((int) _freetypeFace->glyph->bitmap_left, (int) _freetypeFace->glyph->bitmap_top);
				uint advance = (uint) _freetypeFace->glyph->advance.x;

				int dataSize = charSize.X * charSize.Y;
				
				byte[] managedArray = new byte[dataSize];
				Marshal.Copy(_freetypeFace->glyph->bitmap.buffer, managedArray, 0, dataSize);
				Texture texture = Texture.Create(renderer, managedArray, charSize.X, charSize.Y, TextureFilter.Linear, 1);
				
				_glyphs.Add(ch, new FontGlyph(texture, charSize.X, charSize.Y, bearing.X, bearing.Y, advance));
			}
		}
	}

	public FontGlyph GetGlyph(uint glyph)
	{
		return _glyphs[glyph];
	}
}