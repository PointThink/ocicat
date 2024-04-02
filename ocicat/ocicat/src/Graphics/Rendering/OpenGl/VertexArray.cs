using OpenTK.Graphics.OpenGL4;

namespace ocicat.Graphics.Rendering.OpenGl;

public class VertexArray : Rendering.VertexArray
{
	private int _handle;

	private Rendering.IndexBuffer _indexBuffer;
	private Rendering.VertexBuffer _vertexBuffer;
	
	public VertexArray()
	{
		_handle = GL.GenVertexArray();
		GL.BindVertexArray(_handle);
		
		Logging.Log(LogLevel.Ocicat, $"Created OpenGL VertexArray with handle {_handle}");
	}

	~VertexArray()
	{
		Logging.Log(LogLevel.Ocicat, $"Disposed OpenGL VertexArray with handle {_handle}");
		GL.DeleteVertexArray(_handle);
	}
	
	public override void Bind()
	{
		GL.BindVertexArray(_handle);
	}

	public override void Unbind()
	{
		GL.BindVertexArray(0);
	}

	public override void SetVertexBuffer(Rendering.VertexBuffer vertexBuffer)
	{
		Bind();
		vertexBuffer.Bind();

		BufferLayout bufferLayout = vertexBuffer.Layout;

		if (bufferLayout == null)
		{
			Logging.Log(LogLevel.Error, "BufferLayout of vertex buffer was null");
			return;
		}
		
		uint index = 0;
		foreach ( BufferElement element in bufferLayout.BufferElements )
		{
			GL.EnableVertexAttribArray( index );
			GL.VertexAttribPointer( index, (int) element.Count, ShaderTypeToGLEnum(element.DataType), false, (int) bufferLayout.Stride, (int) element.Offset );
			index++;
		}

		_vertexBuffer = vertexBuffer;
		
		GL.BindVertexArray(0);
	}

	public override void SetIndexBuffer(Rendering.IndexBuffer indexBuffer)
	{
		Bind();
		indexBuffer.Bind();
		_indexBuffer = indexBuffer;
	}

	public override Rendering.VertexBuffer GetVertexBuffer()
	{
		return _vertexBuffer;
	}

	public override Rendering.IndexBuffer GetIndexBuffer()
	{
		return _indexBuffer;
	}

	private VertexAttribPointerType ShaderTypeToGLEnum(ShaderDataType shaderDataType)
	{
		switch (shaderDataType)
		{
			case ShaderDataType.Bool:
				return VertexAttribPointerType.Int;
			case ShaderDataType.Float:
				return VertexAttribPointerType.Float;
			case ShaderDataType.Float2:
				return VertexAttribPointerType.Float;
			case ShaderDataType.Float3:
				return VertexAttribPointerType.Float;
			case ShaderDataType.Float4:
				return VertexAttribPointerType.Float;
			case ShaderDataType.Int:
				return VertexAttribPointerType.Int;
			case ShaderDataType.Int2:
				return VertexAttribPointerType.Int;
			case ShaderDataType.Int3:
				return VertexAttribPointerType.Int;
			case ShaderDataType.Int4:
				return VertexAttribPointerType.Int;
		}

		return VertexAttribPointerType.Byte;
	}
}