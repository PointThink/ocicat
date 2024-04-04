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
		
		return view * translation;
	}

	public void ResizeRenderer(int width, int height)
	{
		RenderCommands.ResizeViewport(width, height);
		Camera = new OrthographicCamera(width, height);

		Width = width;
		Height = height;
	}

	public void DrawRect(Vector2 position, Vector2 size, Color color, float rotation = 0)
	{
		Matrix4 projection = Camera.CalculateProjection();
		Matrix4 transform = GenTransform(position, size, rotation);
		
		Primitives.UntexturedMeshShader.Use();
		Primitives.UntexturedMeshShader.UniformMat4("transform", ref transform);
		Primitives.UntexturedMeshShader.UniformMat4("projection", ref projection);
		Primitives.UntexturedMeshShader.Uniform4f("color", color.R, color.G, color.B, color.A);
		
		RenderCommands.DrawIndexed(Primitives.RectangleMesh.VertexArray);
	}
	
	public void DrawCircle(Vector2 center, float radius, int count, Color color)
	{
		List<float> vertecies = new List<float>();
		
		// Generate circle mesh
		for (int i = 0; i < count; i++)
		{
			double degrees = 360f / count * i;

			Vector2 vertex1 = Vector2.FromDegrees((float) degrees);
			Vector2 vertex2 = Vector2.FromDegrees((float) degrees + 360f / count);

			vertecies.Add(0);
			vertecies.Add(0);
			vertecies.Add(vertex1.X);
			vertecies.Add(vertex1.Y);
			vertecies.Add(vertex2.X);
			vertecies.Add(vertex2.Y);
		}
		
		uint[] indicies = new uint[count * 3];

		for (uint i = 0; i < count * 3; i++)
			indicies[i] = i;

		Mesh mesh = new Mesh(this, vertecies.ToArray(), indicies, new BufferLayout(
			[new BufferElement("position", ShaderDataType.Float2)]
		));
		
		Matrix4 projection = Camera.CalculateProjection();
		Matrix4 transform = GenTransform(center, new Vector2(radius, radius), 0);
		
		Primitives.UntexturedMeshShader.Use();
		Primitives.UntexturedMeshShader.UniformMat4("transform", ref transform);
		Primitives.UntexturedMeshShader.UniformMat4("projection", ref projection);
		Primitives.UntexturedMeshShader.Uniform4f("color", color.R, color.G, color.B, color.A);

		RenderCommands.DrawIndexed(mesh.VertexArray);
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
		
		Matrix4 projection = Camera.CalculateProjection();
		Matrix4 transform = GenTransform(new Vector2(0, 0), new Vector2(1, 1), 0);
		
		Primitives.UntexturedMeshShader.Use();
		Primitives.UntexturedMeshShader.UniformMat4("transform", ref transform);
		Primitives.UntexturedMeshShader.UniformMat4("projection", ref projection);
		Primitives.UntexturedMeshShader.Uniform4f("color", color.R, color.G, color.B, color.A);
		
		RenderCommands.DrawIndexed(triangleMesh.VertexArray);
	}
	
	public void DrawRoundedRect(Vector2 position, Vector2 size, float radius, Color color, float rotation = 0)
	{
		Framebuffer framebuffer = Framebuffer.Create(this, (int)size.X, (int)size.Y);
		framebuffer.Bind();

		Color white = Color.CreateFloat(1, 1, 1, 1);

		DrawRect(new Vector2(0, radius), size - new Vector2(0, radius * 2), white);
		DrawRect(new Vector2(radius, 0), new Vector2(size.X - radius * 2, radius), white);
		DrawRect(new Vector2(radius, size.Y - radius), new Vector2(size.X - radius * 2, radius), white);

		DrawCircle(new Vector2(radius, radius), radius, (int)(radius * 0.75f), white);
		DrawCircle(size - new Vector2(radius, radius), radius, (int)(radius * 0.75f), white);
		DrawCircle(new Vector2(size.X - radius, radius), radius, (int)(radius * 0.75f), white);
		DrawCircle(new Vector2(radius, size.Y - radius), radius, (int)(radius * 0.75f), white);

		framebuffer.Unbind();
		DrawRectTextured(position, size, framebuffer.GetTextureAttachment(), color, rotation);
	}
	
	public void DrawRectTextured(Vector2 position, Vector2 size, Texture texture, Color? tint = null, float rotation = 0, bool flipVertical = false, bool flipHorizontal = false)
	{
		if (tint == null)
			tint = Color.CreateFloat(1, 1, 1, 1);
		
		Matrix4 projection = Camera.CalculateProjection();
		Matrix4 transform = GenTransform(position, size, rotation, flipVertical, flipHorizontal);
		
		Primitives.TexturedMeshShader.Use();
        Primitives.TexturedMeshShader.UniformMat4("transform", ref transform);
        Primitives.TexturedMeshShader.UniformMat4("projection", ref projection);;
		Primitives.TexturedMeshShader.Uniform4f("tint", tint.R, tint.G, tint.B, tint.A);

		texture.Bind(0);
		Primitives.TexturedMeshShader.Uniform1i("textureSampler", 0);
		
		RenderCommands.DrawIndexed(Primitives.RectangleMesh.VertexArray);
	}

	public void DrawFontGlyph(FontGlyph glyph, Vector2 position, Color color, float scale = 1, float rotation = 0)
	{
		Matrix4 projection = Camera.CalculateProjection();
		Matrix4 transform = GenTransform(new Vector2(position.X, position.Y - (glyph.SizeY - glyph.BearingY)), new Vector2(glyph.SizeX  * scale, glyph.SizeY * scale), 0);
		
		Primitives.TextShader.Use();
		Primitives.TextShader.UniformMat4("transform", ref transform);
		Primitives.TextShader.UniformMat4("projection", ref projection);
		Primitives.TextShader.Uniform4f("color", color.R, color.G, color.B, color.A);

		glyph.Texture.Bind(0);
		Primitives.TextShader.Uniform1i("textureSampler", 0);
		
		RenderCommands.DrawIndexed(Primitives.RectangleMesh.VertexArray);
	}

	public void DrawText(string text, Font font, Vector2 position, Color? color = null, float scale = 1, float rotation = 0)
	{
		if (color == null)
			color = Color.CreateFloat(1, 1, 1, 1);
		
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
		Matrix4 view = Camera.CalculateView();
		Matrix4 translation = Matrix4.CreateScale(thickness, Vector2.GetDistance(point1, point2), 1) *
		                      Matrix4.CreateRotationZ(Single.DegreesToRadians(Vector2.GetDirection(point1, point2))) *
		                      Matrix4.CreateTranslation(point1.X, point1.Y, 0);


		Matrix4 transform = translation;
		Matrix4 projection = Camera.CalculateProjection();
		
		Primitives.UntexturedMeshShader.Use();
		Primitives.UntexturedMeshShader.UniformMat4("transform", ref transform);
		Primitives.UntexturedMeshShader.UniformMat4("projection", ref projection);
		Primitives.UntexturedMeshShader.Uniform4f("color", color.R, color.G, color.B, color.A);
		
		RenderCommands.DrawIndexed(Primitives.RectangleMesh.VertexArray);
	}
}