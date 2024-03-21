using OpenTK.Mathematics;
using Vector2 = System.Numerics.Vector2;

namespace ocicat.Graphics.Rendering;

public enum RenderingApi
{
	OpenGl
}

public class Renderer
{
	public RenderingApi RenderingApi { get; private set; } 
	public RenderCommands RenderCommands { get; private set; }
	public Primitives Primitives { get; private set; }
	
	public Renderer(RenderingApi renderingApi)
	{
		RenderingApi = renderingApi;
		RenderCommands = RenderCommands.Create(this);
		RenderCommands.Init();

		Primitives = new Primitives(this);
	}

	public void DrawRect(Vector2 position, Vector2 size, Color color)
	{
		Matrix4 projection = Matrix4.CreateOrthographicOffCenter(0, 800, 0, 600, 0f, 1f);
		Matrix4 transform = Matrix4.CreateTranslation(position.X, position.Y, 0);
		Matrix4 scale = Matrix4.CreateScale(size.X, size.Y, 1);

		// transform *= scale;
		
		Primitives.RectShader.Use();
		Primitives.RectShader.UniformMat4("transform", transform);
		Primitives.RectShader.UniformMat4("projection", projection);
		Primitives.RectShader.UniformMat4("scale", scale);
		Primitives.RectShader.Uniform4f("color", color.R, color.G, color.B, color.A);
		
		RenderCommands.DrawIndexed(Primitives.RectangleMesh.VertexArray);
	}
}