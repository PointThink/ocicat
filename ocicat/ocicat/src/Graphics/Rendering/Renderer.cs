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
		
		Primitives.UntexturedRectShader.Use();
		Primitives.UntexturedRectShader.UniformMat4("transform", transform);
		Primitives.UntexturedRectShader.UniformMat4("projection", projection);
		Primitives.UntexturedRectShader.UniformMat4("scale", scale);
		Primitives.UntexturedRectShader.Uniform4f("color", color.R, color.G, color.B, color.A);
		
		RenderCommands.DrawIndexed(Primitives.RectangleMesh.VertexArray);
	}
	
	public void DrawRectTextured(Vector2 position, Vector2 size, Texture texture, Color? tint = null)
	{
		if (tint == null)
			tint = Color.CreateFloat(1, 1, 1, 1);
		
		Matrix4 projection = Matrix4.CreateOrthographicOffCenter(0, 800, 0, 600, 0f, 1f);
		Matrix4 transform = Matrix4.CreateTranslation(position.X, position.Y, 0);
		Matrix4 scale = Matrix4.CreateScale(size.X, size.Y, 1);

		Primitives.TexturedRectShader.Use();
		Primitives.TexturedRectShader.UniformMat4("transform", transform);
		Primitives.TexturedRectShader.UniformMat4("projection", projection);
		Primitives.TexturedRectShader.UniformMat4("scale", scale);
		Primitives.TexturedRectShader.Uniform4f("tint", tint.R, tint.G, tint.B, tint.A);

		texture.Bind(0);
		Primitives.TexturedRectShader.Uniform1i("textureSampler", 0);
		
		RenderCommands.DrawIndexed(Primitives.RectangleMesh.VertexArray);
	}
}