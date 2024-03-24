using System.Text;
using OpenTK.Graphics.ES11;
using OpenTK.Mathematics;
using Vector2 = System.Numerics.Vector2;

namespace ocicat.Graphics.Rendering;

public enum RenderingApi
{
	OpenGl
}

public class Renderer
{
	private Window _window;
	
	public RenderingApi RenderingApi { get; private set; } 
	public RenderCommands RenderCommands { get; private set; }
	
	public Primitives Primitives { get; private set; }

	public Camera Camera { get; private set; }
	
	public Renderer(Window window)
	{
		_window = window;
		
		RenderingApi = window.RenderingApi;
		RenderCommands = RenderCommands.Create(this);
		RenderCommands.Init();

		Primitives = new Primitives(this);

		Camera = new OrthographicCamera(window.Width, window.Height);
	}

	public void DrawRect(Vector2 position, Vector2 size, Color color)
	{
		Matrix4 projection = Camera.CalculateProjection();
		Matrix4 view = Camera.CalculateView();
		Matrix4 transform = Matrix4.CreateTranslation(position.X, position.Y, 0);
		Matrix4 scale = Matrix4.CreateScale(size.X, size.Y, 1);

		transform *= view;
		
		Primitives.UntexturedRectShader.Use();
		Primitives.UntexturedRectShader.UniformMat4("transform", ref transform);
		Primitives.UntexturedRectShader.UniformMat4("projection", ref projection);
		Primitives.UntexturedRectShader.UniformMat4("scale", ref scale);
		Primitives.UntexturedRectShader.Uniform4f("color", color.R, color.G, color.B, color.A);
		
		RenderCommands.DrawIndexed(Primitives.RectangleMesh.VertexArray);
	}
	
	public void DrawRectTextured(Vector2 position, Vector2 size, Texture texture, Color? tint = null)
	{
		if (tint == null)
			tint = Color.CreateFloat(1, 1, 1, 1);
		
		Matrix4 projection = Camera.CalculateProjection();
		Matrix4 view = Camera.CalculateView();
		Matrix4 transform = Matrix4.CreateTranslation(position.X, position.Y, 0);
		Matrix4 scale = Matrix4.CreateScale(size.X, size.Y, 1);

		transform *= view;
		
		Primitives.TexturedRectShader.Use();
        Primitives.TexturedRectShader.UniformMat4("transform", ref transform);
        Primitives.TexturedRectShader.UniformMat4("projection", ref projection);
        Primitives.TexturedRectShader.UniformMat4("scale", ref scale);
		Primitives.TexturedRectShader.Uniform4f("tint", tint.R, tint.G, tint.B, tint.A);

		texture.Bind(0);
		Primitives.TexturedRectShader.Uniform1i("textureSampler", 0);
		
		RenderCommands.DrawIndexed(Primitives.RectangleMesh.VertexArray);
	}

	public void DrawFontGlyph(FontGlyph glyph, Vector2 position, Color color, float scale = 1)
	{
		Matrix4 projection = Camera.CalculateProjection();
		Matrix4 view = Camera.CalculateView();
		Matrix4 transform = Matrix4.CreateTranslation(position.X, position.Y - (glyph.SizeY - glyph.BearingY), 0);
		Matrix4 scaleMat = Matrix4.CreateScale(glyph.SizeX  * scale, glyph.SizeY * scale, 1);

		transform *= view;
		
		Primitives.TextShader.Use();
		Primitives.TextShader.UniformMat4("transform", ref transform);
		Primitives.TextShader.UniformMat4("projection", ref projection);
		Primitives.TextShader.UniformMat4("scale", ref scaleMat);
		Primitives.TextShader.Uniform4f("color", color.R, color.G, color.B, color.A);

		glyph.Texture.Bind(0);
		Primitives.TextShader.Uniform1i("textureSampler", 0);
		
		RenderCommands.DrawIndexed(Primitives.RectangleMesh.VertexArray);
	}

	public void DrawText(string text, Font font, Vector2 position, Color? color = null, float scale = 1)
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

	public void DrawCircle(Vector2 center, float radius, int count, Color color)
	{
		List<float> vertecies = new List<float>();
		
		// Generate circle mesh
		for (int i = 0; i < count; i++)
		{
			double degrees = (360d / count) * i;

			Vector2 vertex1 = Vector2.Normalize((float) degrees, radius);
			Vector2 vertex2 = Vector2.Normalize((float) degrees + (360f / count), radius);
			
			vertecies.Add(center.X);
			vertecies.Add(center.Y);
			vertecies.Add(vertex1.X + center.X);
			vertecies.Add(vertex1.Y + center.Y);
			vertecies.Add(vertex2.X + center.X);
			vertecies.Add(vertex2.Y + center.Y);
		}

		uint[] indicies = new uint[vertecies.Count / 2];

		for (uint i = 0; i < vertecies.Count / 2; i++)
		{
			indicies[i] = i;
		}

		Mesh mesh = new Mesh(this, vertecies.ToArray(), indicies, new BufferLayout(
			[new BufferElement("position", ShaderDataType.Float2)]
		));
		
		Matrix4 projection = Camera.CalculateProjection();
		Matrix4 view = Camera.CalculateView();
		Matrix4 transform = Matrix4.CreateTranslation(0, 0, 0);
		Matrix4 scaleMat = Matrix4.CreateScale(1, 1, 1);
		
		Primitives.UntexturedRectShader.Use();
		Primitives.UntexturedRectShader.UniformMat4("transform", ref transform);
		Primitives.UntexturedRectShader.UniformMat4("projection", ref projection);
		Primitives.UntexturedRectShader.UniformMat4("scale", ref scaleMat);
		Primitives.UntexturedRectShader.Uniform4f("color", color.R, color.G, color.B, color.A);
		
		RenderCommands.DrawIndexed(mesh.VertexArray);
	}
}