using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

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
			GL.GetActiveUniform(_handle, i, 64, out int lenght, out int size, out ActiveUniformType uniformType, out string name);
			_uniformLocations.Add(name, GL.GetUniformLocation(_handle, name));
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
        GL.ProgramUniform1(_handle, _uniformLocations[name], i1);
    }

	public override void Uniform2i(string name, int i1, int i2)
	{
        GL.ProgramUniform2(_handle, _uniformLocations[name], i1, i2);
    }

	public override void Uniform3i(string name, int i1, int i2, int i3)
	{
        GL.ProgramUniform3(_handle, _uniformLocations[name], i1, i2, i3);
    }

	public override void Uniform4i(string name, int i1, int i2, int i3, int i4)
	{
        GL.ProgramUniform4(_handle, _uniformLocations[name], i1, i2, i3, i4);
    }

	public override void Uniform1f(string name, float f1)
	{
        GL.ProgramUniform1(_handle, _uniformLocations[name], f1);
    }

	public override void Uniform2f(string name, float f1, float f2)
	{

        GL.ProgramUniform2(_handle, _uniformLocations[name], f1, f2);
    }

	public override void Uniform3f(string name, float f1, float f2, float f3)
	{
        GL.ProgramUniform3(_handle, _uniformLocations[name], f1, f2, f3);
    }

	public override void Uniform4f(string name, float f1, float f2, float f3, float f4)
	{
		GL.ProgramUniform4(_handle, _uniformLocations[name], f1, f2, f3, f4);
	}

	public override void UniformMat4(string name, ref Matrix4 matrix4)
	{
		GL.ProgramUniformMatrix4(_handle, _uniformLocations[name], true, ref matrix4);
	}
}