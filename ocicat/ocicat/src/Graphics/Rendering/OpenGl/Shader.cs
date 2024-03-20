using OpenTK.Graphics.OpenGL4;

namespace ocicat.Graphics.Rendering.OpenGl;

public class Shader : Rendering.Shader
{
	private int _handle;
	
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
}