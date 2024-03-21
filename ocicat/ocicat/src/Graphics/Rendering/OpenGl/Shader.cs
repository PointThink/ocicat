using OpenTK.Graphics.OpenGL4;

namespace ocicat.Graphics.Rendering.OpenGl;

public class Shader : Rendering.Shader
{
	private int _handle;

	private Dictionary<string, int> _uniformLocations;
	
	public Shader(string vertexSource, string fragSource)
	{
		int vertexShader = GL.CreateShader(ShaderType.VertexShader);
		int fragShader = GL.CreateShader(ShaderType.FragmentShader);
		
		GL.ShaderSource(vertexShader, vertexSource);
		GL.ShaderSource(fragShader, fragSource);
		
		CompileShader(vertexShader);
		CompileShader(fragShader);

		_handle = GL.CreateProgram();
		
		GL.AttachShader(_handle, vertexShader);
		GL.AttachShader(_handle, fragShader);
		
		GL.LinkProgram(_handle);
		
		GL.GetProgram(_handle, GetProgramParameterName.LinkStatus, out int success);
		if (success == 0)
		{
			string infoLog = GL.GetProgramInfoLog(_handle);
			Logging.Log(LogLevel.Error, $"Error linking shader: {infoLog}");
		}

		_uniformLocations = new Dictionary<string, int>();

		GL.GetProgram(_handle, GetProgramParameterName.ActiveUniforms, out int uniforms);

		for (int i = 0; i < uniforms; i++)
		{
			GL.GetActiveUniform(_handle, i, 32, out int lenght, out int size, out ActiveUniformType uniformType, out string name);
			_uniformLocations.Add(name, i);
		}
	}

	private void CompileShader(int shaderId)
	{
		GL.CompileShader(shaderId);
		
		GL.GetShader(shaderId, ShaderParameter.CompileStatus, out int success);
		if (success == 0)
		{
			string infoLog = GL.GetShaderInfoLog(shaderId);
			Logging.Log(LogLevel.Error, $"Error compiling shader: {infoLog}");
		}
	}
	
	public override void Use()
	{
		GL.UseProgram(_handle);
	}

	public override void Uniform1i(string name, int i1)
	{
		GL.Uniform1(_uniformLocations[name], i1);
	}

	public override void Uniform2i(string name, int i1, int i2)
	{
		GL.Uniform2(_uniformLocations[name], i1, i2);
	}

	public override void Uniform3i(string name, int i1, int i2, int i3)
	{
		GL.Uniform3(_uniformLocations[name], i1, i2, i3);
	}

	public override void Uniform1f(string name, float f1)
	{
		GL.Uniform1(_uniformLocations[name], f1);
	}

	public override void Uniform2f(string name, float f1, float f2)
	{
		GL.Uniform2(_uniformLocations[name], f1, f2);
	}

	public override void Uniform3f(string name, float f1, float f2, float f3)
	{
		GL.Uniform3(_uniformLocations[name], f1, f2, f3);
	}
}