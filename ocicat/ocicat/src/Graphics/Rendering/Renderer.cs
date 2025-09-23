using System.Text;
using OpenTK.Mathematics;

namespace ocicat.Graphics.Rendering;

public enum RenderingApi
{
	OpenGl
}

public class Renderer
{
	public Window Window { get; private set; }
	
	public RenderingApi RenderingApi { get; private set; } 
	public RenderCommands RenderCommands { get; private set; }
	
	public Primitives Primitives { get; private set; }

	public Camera Camera;
	
	public int Width { get; private set; }
	public int Height { get; private set; }

	public Renderer(Window window)
	{
		Logging.Log(LogLevel.Ocicat, $"Initializing renderer with {Enum.GetName(typeof(RenderingApi), window.RenderingApi)}");
		
		Window = window;
		
		RenderingApi = window.RenderingApi;
		RenderCommands = RenderCommands.Create(this);
		RenderCommands.Init();

		Primitives = new Primitives(this);

		Width = window.Width;
		Height = window.Height;
		
		Camera = new OrthographicCamera(window.Width, window.Height);
	}

	public void BeginDrawing()
	{
	}

	public void EndDrawing()
	{
	}

	public void ClearScreen(Color color)
	{
		RenderCommands.SetClearColor(color.R, color.G, color.B, color.A);
		RenderCommands.ClearScreen();
	}

	private Matrix4 GenTransform(Vector2 position, Vector2 size, float rotation, bool flipVertical = false, bool flipHorizontal = false)
	{
		if (flipVertical)
		{
			position.Y += size.Y;
			size.Y *= -1;
		}
		
		if (flipHorizontal)
		{
			position.X += size.X;
			size.X *= -1;
		}
		
		Matrix4 view = Camera.CalculateView();
		Matrix4 translation = Matrix4.CreateScale(size.X, size.Y, 1) *
		                      Matrix4.CreateTranslation(-size.X / 2, -size.Y / 2, 0) *
		                      Matrix4.CreateRotationZ(Single.DegreesToRadians(rotation)) *
		                      Matrix4.CreateTranslation(position.X + size.X / 2, position.Y + size.Y / 2, 0);
		
		return translation * view;
	}

	public void ResizeRenderer(int width, int height)
	{
		RenderCommands.ResizeViewport(width, height);
		Camera = new OrthographicCamera(width, height);

		Width = width;
		Height = height;
	}

	public void DrawMesh(Mesh mesh, Vector2 position, Vector2 size, Color color, float rotation)
	{
		Matrix4 projection = Camera.CalculateProjection();
		Matrix4 transform = GenTransform(position, size, rotation, false, false);
		
		Primitives.UntexturedMeshShader.Use();
		Primitives.UntexturedMeshShader.UniformMat4("transform", ref transform);
		Primitives.UntexturedMeshShader.UniformMat4("projection", ref projection);
		Primitives.UntexturedMeshShader.Uniform4f("color", color.R, color.G, color.B, color.A);
		
		RenderCommands.DrawIndexed(mesh.VertexArray);
	}
	
	public void DrawTexturedMesh(Mesh mesh, Texture texture, Vector2 position, Vector2 size, Color tint, float rotation,
		bool flipVertical, bool flipHorizontal
		)
	{
		
		Matrix4 projection = Camera.CalculateProjection();
		Matrix4 transform = GenTransform(position, size, rotation, flipVertical, flipHorizontal);

		OrthographicCamera camera = (OrthographicCamera)Camera;
		
		Primitives.TexturedMeshShader.Use();
		Primitives.TexturedMeshShader.UniformMat4("transform", ref transform);
		Primitives.TexturedMeshShader.UniformMat4("projection", ref projection);;
		Primitives.TexturedMeshShader.Uniform4f("tint", tint.R, tint.G, tint.B, tint.A);

		texture.Bind(0);
		Primitives.TexturedMeshShader.Uniform1i("textureSampler", 0);
		
		RenderCommands.DrawIndexed(mesh.VertexArray);
	}

	public void DrawRect(Vector2 position, Vector2 size, Color color, float rotation = 0)
	{
		DrawMesh(Primitives.RectangleMesh, position, size, color, rotation);
	}
	
	public void DrawCircle(Vector2 center, float radius, int count, Color color)
	{
		Mesh circleMesh = Mesh.GenCircleMesh(this, count);
		DrawMesh(circleMesh, center, new Vector2(radius, radius), color, 0);
	}
	
	public void DrawTriangle(Vector2 point1, Vector2 point2, Vector2 point3, Color color)
	{
		float[] verticies =
		{
			point1.X, point1.Y,
			point2.X, point2.Y,
			point3.X, point3.Y
		};

		Mesh triangleMesh = new Mesh(this, verticies, [0, 1, 2], new BufferLayout(
			[new BufferElement("position", ShaderDataType.Float2)]
		));
		
		DrawMesh(triangleMesh, new Vector2(0, 0), new Vector2(1, 1), color, 0);
	}

	
	public void DrawRectTextured(Vector2 position, Vector2 size, Texture texture, Color tint, float rotation = 0, bool flipVertical = false, bool flipHorizontal = false)
	{
		DrawTexturedMesh(Primitives.RectangleMesh, texture, position, size, tint, rotation, flipVertical, flipHorizontal);
	}

	public void DrawSprite(Vector2 position, Sprite sprite, Color tint, float scale = 1, float rotation = 0,
		bool flipVertical = false, bool flipHorizontal = false)
	{
		
		Matrix4 projection = Camera.CalculateProjection();
		Matrix4 transform = GenTransform(position, sprite.Size * new Vector2(scale, scale), rotation, flipVertical, flipHorizontal);

		OrthographicCamera camera = (OrthographicCamera)Camera;
		
		Primitives.SpritesheetShader.Use();
		Primitives.SpritesheetShader.UniformMat4("transform", ref transform);
		Primitives.SpritesheetShader.UniformMat4("projection", ref projection);;
		Primitives.SpritesheetShader.Uniform4f("tint", tint.R, tint.G, tint.B, tint.A);

		sprite.Spritesheet.SpritesheetTexture.Bind(0);
		Primitives.SpritesheetShader.Uniform1i("textureSampler", 0);
		Primitives.SpritesheetShader.Uniform2f("sheetSize", sprite.Spritesheet.SpritesheetTexture.GetWidth(), sprite.Spritesheet.SpritesheetTexture.GetHeight());
		Primitives.SpritesheetShader.Uniform2f("spriteOffset", sprite.Offset.X, sprite.Offset.Y);
		Primitives.SpritesheetShader.Uniform2f("spriteSize", sprite.Size.X, sprite.Size.Y);
		
		RenderCommands.DrawIndexed(Primitives.RectangleMesh.VertexArray);
	}

	public void DrawFontGlyph(FontGlyph glyph, Vector2 position, Color color, float scale = 1, float rotation = 0)
	{
		Matrix4 projection = Camera.CalculateProjection();
		Matrix4 transform = GenTransform(new Vector2(position.X, position.Y + (glyph.FontSize - glyph.BearingY) * scale), new Vector2(glyph.SizeX  * scale, glyph.SizeY * scale), 0);
		
		Primitives.TextShader.Use();
		Primitives.TextShader.UniformMat4("transform", ref transform);
		Primitives.TextShader.UniformMat4("projection", ref projection);
		Primitives.TextShader.Uniform4f("color", color.R, color.G, color.B, color.A);

		glyph.Texture.Bind(0);
		Primitives.TextShader.Uniform1i("textureSampler", 0);
		
		RenderCommands.DrawIndexed(Primitives.RectangleMesh.VertexArray);
	}

	public void DrawText(string text, Font font, Vector2 position, Color color, float scale = 1, float rotation = 0)
	{	
		byte[] characters = Encoding.ASCII.GetBytes(text);
		Vector2 currentPosition = position;
		
		foreach (byte character in characters)
		{
			if (character == 32) // space
				currentPosition.X += font.SpaceSize * scale;
			else if (character == 9) // tab
				currentPosition.X += font.SpaceSize * 4 * scale;
			else
			{
				FontGlyph glyph = font.GetGlyph(character);
				
				DrawFontGlyph(glyph, currentPosition, color, scale);
				currentPosition.X += glyph.Advance * scale;
			}
		}
	}

	public void DrawLine(Vector2 point1, Vector2 point2, float thickness, Color color)
	{
		Matrix4 translation = Matrix4.CreateScale(Vector2.GetDistance(point1, point2), thickness, 1) *
		                      Matrix4.CreateRotationZ(Single.DegreesToRadians(Vector2.GetDirection(point1, point2))) *
		                      Matrix4.CreateTranslation(point1.X, point1.Y, 0);


		Matrix4 transform = translation * Camera.CalculateView();
		Matrix4 projection = Camera.CalculateProjection();
		
		Primitives.UntexturedMeshShader.Use();
		Primitives.UntexturedMeshShader.UniformMat4("transform", ref transform);
		Primitives.UntexturedMeshShader.UniformMat4("projection", ref projection);
		Primitives.UntexturedMeshShader.Uniform4f("color", color.R, color.G, color.B, color.A);
		
		RenderCommands.DrawIndexed(Primitives.RectangleMesh.VertexArray);
	}
}