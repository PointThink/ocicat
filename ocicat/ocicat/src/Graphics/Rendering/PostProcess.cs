namespace ocicat.Graphics.Rendering;

public class PostProcessShader
{
	public static string VertShader = @"#version 330 core
layout (location = 0) in vec2 aPosition;
layout (location = 1) in vec2 texCoords;

uniform mat4 projection;
uniform mat4 transform;

out vec2 vTexCoords;

void main()
{
    gl_Position = vec4(aPosition, 0.0, 1.0) * transform * projection;
	vTexCoords = texCoords;
}";
	
	public Shader Shader { get; private set; }

	public PostProcessShader(Renderer renderer, string fragShaderPath)
	{
		Shader = Shader.Create(renderer, VertShader, fragShaderPath);
	}
}

public class PostProcessPass
{
	public PostProcessPass(PostProcessShader shader)
	{
		
	}
}