using ocicat;
using ocicat.Graphics;
using ocicat.Graphics.Rendering;
using OpenTK.Graphics.OpenGL;

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
		
		GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
		
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
		
		int VertexArrayObject = GL.GenVertexArray();
		GL.BindVertexArray(VertexArrayObject);
		
		GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
		GL.EnableVertexAttribArray(0);

		vertexBuffer.Bind();
		indexBuffer.Bind();
		
		shader.Use();
		
		while (!window.ShouldClose())
		{
			window.HandleEvents();
			
			GL.Clear(ClearBufferMask.ColorBufferBit);
			GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, 0);			
			
			window.SwapBuffers();
		}
	}
}