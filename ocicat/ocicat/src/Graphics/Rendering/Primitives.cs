namespace ocicat.Graphics.Rendering;

public class Primitives
{
	public Mesh RectangleMesh;
	public Shader RectShader;
	
	public Primitives(Renderer renderer)
	{
		float[] rectVertices = {
			0, 0, 0, 0,
			1, 0, 1, 0,
			1, 1, 1, 1,
			0, 1, 0, 1
		};

		uint[] rectIndices =
		{
			0, 1, 2, 2, 3, 0
		};

		RectangleMesh = new Mesh(
			renderer, rectVertices, rectIndices,
			new BufferLayout([
				new BufferElement("position", ShaderDataType.Float2),
				new BufferElement("uv", ShaderDataType.Float2)
			])
		);
		
		string vertShader = @"#version 330 core
layout (location = 0) in vec3 aPosition;

uniform mat4 projection;
uniform mat4 transform;
uniform mat4 scale;

void main()
{
    gl_Position = vec4(aPosition, 1.0) * scale * transform * projection;
}";

		string fragShader = @"#version 330 core
out vec4 FragColor;
uniform vec4 color;

void main()
{
    FragColor = color;
}";
		
		RectShader = Shader.Create(renderer, vertShader, fragShader);
	}
}