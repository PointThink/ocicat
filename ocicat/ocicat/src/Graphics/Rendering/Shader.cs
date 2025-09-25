using OpenTK.Mathematics;

namespace ocicat.Graphics.Rendering;

public abstract class Shader
{
	protected Shader() {}
	
	public abstract void Use();

	public abstract void Uniform1i(string name, int i1);
	public abstract void Uniform2i(string name, int i1, int i2);
	public abstract void Uniform3i(string name, int i1, int i2, int i3);
	public abstract void Uniform4i(string name, int i1, int i2, int i3, int i4);
	
	public abstract void Uniform1f(string name, float i1);
	public abstract void Uniform2f(string name, float i1, float i2);
	public abstract void Uniform3f(string name, float i1, float i2, float i3);
	public abstract void Uniform4f(string name, float i1, float i2, float i3, float i4);

	public abstract void UniformMat4(string name, Matrix4 matrix4);

	public static Shader Create(Renderer renderer, string vertexSoucre, string fragSource)
	{
		switch (renderer.RenderingApi)
		{
			case RenderingApi.OpenGl:
				return new OpenGl.Shader(vertexSoucre, fragSource);
		}

		throw new ArgumentException("Invalid RenderingApi");
	}
}