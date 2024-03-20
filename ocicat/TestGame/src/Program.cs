using ocicat;
using ocicat.Graphics;

using OpenTK.Graphics.OpenGL;

namespace TestGame;

class Program
{
	public class Shader
	{
		int Handle;

		public Shader(string vertex, string fragment)
		{
			int vertexShader;
			int fragmentShader;
			
			vertexShader = GL.CreateShader(ShaderType.VertexShader);
			GL.ShaderSource(vertexShader, vertex);

			fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
			GL.ShaderSource(fragmentShader, fragment);
			
		
			GL.CompileShader(vertexShader);
		
			GL.GetShader(vertexShader, ShaderParameter.CompileStatus, out int success1);
			if (success1 == 0)
			{
				string infoLog = GL.GetShaderInfoLog(vertexShader);
				Console.WriteLine(infoLog);
			}
		
			GL.CompileShader(fragmentShader);

			GL.GetShader(fragmentShader, ShaderParameter.CompileStatus, out int success2);
			if (success2 == 0)
			{
				string infoLog = GL.GetShaderInfoLog(fragmentShader);
				Console.WriteLine(infoLog);
			}
		
		
			Handle = GL.CreateProgram();

			GL.AttachShader(Handle, vertexShader);
			GL.AttachShader(Handle, fragmentShader);

			GL.LinkProgram(Handle);

			GL.GetProgram(Handle, GetProgramParameterName.LinkStatus, out int success3);
			if (success3 == 0)
			{
				string infoLog = GL.GetProgramInfoLog(Handle);
				Console.WriteLine(infoLog);
			}
		}
		
		public void Use()
		{
			GL.UseProgram(Handle);
		}
	}
	
	static void Main(string[] args)
	{
		Window window = new Window("Hello", 800, 600);

		Logging.LogLevel = LogLevel.Developer;
		
		Logging.Log(LogLevel.Developer, "Hello");
		Logging.Log(LogLevel.Info, "Hello");
		Logging.Log(LogLevel.Warning, "Hello");
		Logging.Log(LogLevel.Error, "Hello");
		
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

		int ElementBufferObject;
		ElementBufferObject = GL.GenBuffer();
		GL.BindBuffer(BufferTarget.ElementArrayBuffer, ElementBufferObject);
		GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);
		
		int VertexBufferObject;
		VertexBufferObject = GL.GenBuffer();

		GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
		GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

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

		Shader shader = new Shader(vertShader, fragShader);
		
		int VertexArrayObject = GL.GenVertexArray();
		GL.BindVertexArray(VertexArrayObject);
		
		GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
		GL.EnableVertexAttribArray(0);

		GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
		GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

		GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
		GL.EnableVertexAttribArray(0);

		GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
		GL.BindBuffer(BufferTarget.ElementArrayBuffer, ElementBufferObject);
		
		shader.Use();
		
		while (!window.ShouldClose())
		{
			window.HandleEvents();
			
			GL.Clear(ClearBufferMask.ColorBufferBit);
			GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, 0);			
			
			window.SwapBuffers();
		}
		
		GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
		GL.DeleteBuffer(VertexBufferObject);
	}
}