namespace ocicat.Graphics.Rendering;

public abstract class Shader
{
	protected Shader() {}
	
	public abstract void Use();

	public static Shader? Create(Renderer renderer, string vertexSoucre, string fragSource)
	{
		switch (renderer.RenderingApi)
		{
			case RenderingApi.OpenGl:
				return new OpenGl.Shader(vertexSoucre, fragSource);
		}

		return null;
	}
}