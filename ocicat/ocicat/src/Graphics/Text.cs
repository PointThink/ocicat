using System.Runtime.InteropServices;
using System.Text;
using FreeTypeSharp.Native;
using ocicat.Graphics.Rendering;
using OpenTK.Mathematics;
namespace ocicat.Graphics;

/// <summary>
/// A single character from the font. 
/// </summary>
public class FontGlyphInternal
{
	public FontGlyphInternal(Texture texture, int sizeX, int sizeY, int bearingX, int bearingY, int advanceX, int advanceY, uint fontSize)
	{
		Texture = texture;
		SizeX = sizeX;
		SizeY = sizeY;
		BearingX = bearingX;
		BearingY = bearingY;
		AdvanceX = advanceX;
		AdvanceY = advanceY;
		FontSize = fontSize;
	}

	public readonly Texture Texture;

	public readonly int SizeX;
	public readonly int SizeY;
	public readonly int BearingX;
	public readonly int BearingY;
	public readonly int AdvanceX;
	public readonly int AdvanceY;
	public readonly uint FontSize;
}

public class FontGlyph
{
	public FontGlyph(Texture texture, Vector2 startUVs, Vector2 endUVs, int sizeX, int sizeY, int bearingX, int bearingY, int advanceX, int advanceY, uint fontSize)
	{
		Texture = texture;
		SizeX = sizeX;
		SizeY = sizeY;
		BearingX = bearingX;
		BearingY = bearingY;
		AdvanceX = advanceX;
		AdvanceY = advanceY;
		FontSize = fontSize;
		StartUVs = startUVs;
		EndUVs = endUVs;
	}

	public readonly Texture Texture;

	public readonly Vector2 StartUVs;
	public readonly Vector2 EndUVs;
	public readonly int SizeX;
	public readonly int SizeY;
	public readonly int BearingX;
	public readonly int BearingY;
	public readonly int AdvanceX;
	public readonly int AdvanceY;
	public readonly uint FontSize;
}

public unsafe class Font
{
	public int SpaceSize = 10;
	public readonly uint FontSize;

	private Dictionary<uint, FontGlyph> _glyphs = new();
	public readonly RenderTarget TextureAtlasRT; 

	public Font(Renderer renderer, string path, uint size)
	{
		Dictionary<uint, FontGlyphInternal> glyphs = new();

		IntPtr freetype;
		FT.FT_Init_FreeType(out freetype);

		FT.FT_New_Face(freetype, path, 0, out IntPtr tempFace);
		FT_FaceRec* freetypeFace = (FT_FaceRec*)tempFace;

		FT.FT_Set_Pixel_Sizes((IntPtr)freetypeFace, 0, size);

		FT.FT_Library_SetLcdFilter(freetype, FT_LcdFilter.FT_LCD_FILTER_MAX);

		FontSize = size;

		// Load all glyphs
		for (uint ch = 0; ch < 128; ch++)
		{
			if (FT.FT_Load_Char((IntPtr)freetypeFace, ch, FT.FT_LOAD_RENDER) == FT_Error.FT_Err_Ok)
			{
				FT_Error error = FT.FT_Render_Glyph((IntPtr)freetypeFace->glyph, FT_Render_Mode.FT_RENDER_MODE_SDF);

				if (error != FT_Error.FT_Err_Ok)
				{
					Logging.Log(LogLevel.Error, $"Error loading font, error code {error}");
					continue;
				}

				if (freetypeFace->glyph->bitmap.buffer == IntPtr.Zero)
				{
					Logging.Log(LogLevel.Warning,
						$"_freetypeFace->glyph->bitmap.buffer for character {ch} is null but Freetype didn't report any errors");
					continue;
				}

				Vector2i charSize = new Vector2i((int)freetypeFace->glyph->bitmap.width,
					(int)freetypeFace->glyph->bitmap.rows);
				Vector2i bearing = new Vector2i((int)freetypeFace->glyph->bitmap_left,
					(int)freetypeFace->glyph->bitmap_top);
				int advanceX = (int)(freetypeFace->glyph->metrics.horiAdvance >> 6);
				int advanceY = (int)(freetypeFace->glyph->metrics.vertAdvance >> 6);

				int dataSize = charSize.X * charSize.Y * 1;

				byte[] managedArray = new byte[dataSize];
				Marshal.Copy(freetypeFace->glyph->bitmap.buffer, managedArray, 0, dataSize);
				Texture texture = Texture.Create(renderer, managedArray, charSize.X, charSize.Y, TextureFilter.Linear,
					1);

				glyphs.Add(ch, new FontGlyphInternal(texture, charSize.X, charSize.Y, bearing.X, bearing.Y, advanceX, advanceY, size));
			}
		}

		// generate texture atlas for the glyphs
		int sizeX = 0;
		int sizeY = 0;

		foreach (FontGlyphInternal glyph in glyphs.Values)
		{
			sizeX += glyph.SizeX;

			if (glyph.SizeY > sizeY)
				sizeY = glyph.SizeY;
		}

		TextureAtlasRT = new RenderTarget(renderer, sizeX, sizeY);
		renderer.RenderTarget = TextureAtlasRT;

		float offsetX = 0;

		foreach (KeyValuePair<uint, FontGlyphInternal> pair in glyphs)
		{
			FontGlyphInternal glyph = pair.Value;
			renderer.DrawRectTexturedUnbatched(new Vector2(-TextureAtlasRT.Camera.Width / 2 + offsetX, -TextureAtlasRT.Height / 2), new Vector2(glyph.SizeX, glyph.SizeY), glyph.Texture, Color.White);

			_glyphs.Add(pair.Key, new FontGlyph(
				TextureAtlasRT.Texture,
				new Vector2(offsetX / sizeX, 0), new Vector2((offsetX + glyph.SizeX) / sizeX, (float) 	glyph.SizeY / sizeY),
				glyph.SizeX, glyph.SizeY, glyph.BearingX, glyph.BearingY, glyph.AdvanceX, glyph.AdvanceY, glyph.FontSize 
			));

			offsetX += glyph.SizeX;
		}

		renderer.RenderTarget = null;

		FT.FT_Done_Face((IntPtr)freetypeFace);
		FT.FT_Done_Library(freetype);
	}

	/// <param name="glyph">The ASCII representation of the characters.</param>
	/// <returns>A glyph</returns>
	public FontGlyph GetGlyph(uint glyph)
	{
		return _glyphs[glyph];
	}

	public float GetStringSize(string text)
	{

		byte[] characters = Encoding.ASCII.GetBytes(text);
		Vector2 currentPosition = new Vector2();

		foreach (byte character in characters)
		{
			if (character == 32) // space
				currentPosition.X += SpaceSize;
			else if (character == 9) // tab
				currentPosition.X += SpaceSize * 4;
			else
			{
				FontGlyph glyph = GetGlyph(character);
				currentPosition.X += glyph.AdvanceX;
			}
		}

		return currentPosition.X;
	}
}