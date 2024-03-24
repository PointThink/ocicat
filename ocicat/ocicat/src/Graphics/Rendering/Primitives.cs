namespace ocicat.Graphics.Rendering;

public class Primitives
{
	public Mesh RectangleMesh;
	public Shader UntexturedRectShader;
	public Shader TexturedRectShader;
	public Shader TextShader;
	
	public Primitives(Renderer renderer)
	{
		float[] rectVertices = {
			0, 0, 0, 1,
			1, 0, 1, 1,
			1, 1, 1, 0,
			0, 1, 0, 0
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
		
		string untexturedVertShader = @"#version 330 core
layout (location = 0) in vec2 aPosition;

uniform mat4 projection;
uniform mat4 transform;
uniform mat4 scale;

void main()
{
    gl_Position = vec4(aPosition, 0.0, 1.0) * scale * transform * projection;
}";

		string untexturedFragShader = @"#version 330 core
out vec4 FragColor;
uniform vec4 color;

void main()
{
    FragColor = color;
}";
		
		string texturedVertShader = @"#version 330 core
layout (location = 0) in vec2 aPosition;
layout (location = 1) in vec2 texCoords;

uniform mat4 projection;
uniform mat4 transform;
uniform mat4 scale;

out vec2 vTexCoords;

void main()
{
    gl_Position = vec4(aPosition, 0.0, 1.0) * scale * transform * projection;
	vTexCoords = texCoords;
}";

		string texturedFragShader = @"#version 330 core
out vec4 FragColor;

uniform sampler2D textureSampler;
uniform vec4 tint;

in vec2 vTexCoords;

void main()
{
    FragColor = texture(textureSampler, vTexCoords) * tint;
}";

		string fontFragShader = @"#version 330 core
out vec4 FragColor;

uniform sampler2D textureSampler;
uniform vec4 color;

in vec2 vTexCoords;

void main()
{
	vec4 sampled = vec4(1, 1, 1, texture(textureSampler, vTexCoords));
    FragColor = sampled * color;
}";
		
		UntexturedRectShader = Shader.Create(renderer, untexturedVertShader, untexturedFragShader);
		TexturedRectShader = Shader.Create(renderer, texturedVertShader, texturedFragShader);
		TextShader = Shader.Create(renderer, texturedVertShader, fontFragShader);
	}
}