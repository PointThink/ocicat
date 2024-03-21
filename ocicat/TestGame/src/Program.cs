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
			-0.5f, -0.5f, 0.0f, //Bottom-left vertex
			0.5f, -0.5f, 0.0f, //Bottom-right vertex
			0.0f,  0.5f, 0.0f  //Top vertex
		};

		uint[] indices =
		{
			0, 1, 2, 3
		};
		
		VertexBuffer vertexBuffer = VertexBuffer.Create(renderer, vertices);
		vertexBuffer.Layout = new BufferLayout([
			new BufferElement("position", ShaderDataType.Float3)
		]);

		IndexBuffer indexBuffer = IndexBuffer.Create(renderer, indices);

		string vertShader = @"#version 330 core
layout (location = 0) in vec3 aPosition;

void main()
{
    gl_Position = vec4(aPosition, 1.0);
}";

		string fragShader = @"#version 330 core
out vec4 FragColor;

void main()
{
    FragColor = vec4(1.0f, 0.5f, 0.2f, 1.0f);
}";

		Shader shader = Shader.Create(renderer, vertShader, fragShader);
		
		VertexArray vertexArray = VertexArray.Create(renderer);
		
		vertexArray.SetVertexBuffer(vertexBuffer);
		vertexArray.SetIndexBuffer(indexBuffer);
		
		vertexBuffer.Bind();
		indexBuffer.Bind();
		
		shader.Use();
		
		renderer.RenderCommands.SetClearColor(0.2f, 0.2f, 0.2f, 1f);
		
		while (!window.ShouldClose())
		{
			window.HandleEvents();
			
			renderer.RenderCommands.ClearScreen();
			renderer.RenderCommands.DrawIndexed(vertexArray);
			
			window.SwapBuffers();
		}
	}
}