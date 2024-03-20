using OpenTK.Graphics.OpenGL4;

namespace ocicat.Graphics.Rendering.OpenGl;

public class VertexBuffer : Rendering.VertexBuffer
{
	private int _handle;
	
	public VertexBuffer(float[] data)
	{
		_handle = GL.GenBuffer();
		GL.BindBuffer(BufferTarget.ArrayBuffer, _handle);
		GL.BufferData(BufferTarget.ArrayBuffer, data.Length * sizeof(float), data, BufferUsageHint.StaticDraw);
	}

	~VertexBuffer()
	{
		GL.DeleteBuffer(_handle);
	}

	public override void Bind()
	{
		GL.BindBuffer(BufferTarget.ArrayBuffer, _handle);
	}

	public override void Unbind()
	{
		GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
	}
}