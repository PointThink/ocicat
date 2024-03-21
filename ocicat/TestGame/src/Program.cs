using ocicat;
using ocicat.Graphics;
using ocicat.Graphics.Rendering;

namespace TestGame;

class Program
{
	static void Main(string[] args)
	{
		Window window = new Window("Hello", 800, 600);

		Logging.LogLevel = LogLevel.Developer;
		
		Logging.Log(LogLevel.Developer, "Hello");
		Logging.Log(LogLevel.Info, "Hello");
		Logging.Log(LogLevel.Warning, "Hello");
		Logging.Log(LogLevel.Error, "Hello");

		Renderer renderer = new Renderer(RenderingApi.OpenGl);
		
		float[] vertices = {
			-.5f, -.5f, 0, 0,
			.5f, -.5f, 1, 0,
			.5f, .5f, 1, 1,
			-.5f, .5f, 0, 1
		};

		uint[] indices =
		{
			0, 1, 2, 2, 3, 0
		};

		Mesh mesh = new Mesh(
			renderer, vertices, indices,
			new BufferLayout([
				new BufferElement("position", ShaderDataType.Float2),
				new BufferElement("uv", ShaderDataType.Float2)
			])
		);

		string vertShader = @"#version 330 core
layout (location = 0) in vec3 aPosition;
layout (location = 1) in vec2 uv;
out vec2 texUv;

void main()
{
    gl_Position = vec4(aPosition, 1.0);
	texUv = uv;
}";

		string fragShader = @"#version 330 core
out vec4 FragColor;
in vec2 texUv;
uniform sampler2D textureId;

void main()
{
    FragColor = texture(textureId, texUv);
}";

		Shader shader = Shader.Create(renderer, vertShader, fragShader);
		
		shader.Use();

		Texture texture = Texture.Create(renderer, "image.jpg");
		texture.Bind(0);
		shader.Uniform1i("textureId", 0);
		
		renderer.RenderCommands.SetClearColor(0.2f, 0.2f, 0.2f, 1f);
		
		while (!window.ShouldClose())
		{
			window.HandleEvents();
			
			renderer.RenderCommands.ClearScreen();
			renderer.RenderCommands.DrawIndexed(mesh.VertexArray);
			
			window.SwapBuffers();
		}
	}
}