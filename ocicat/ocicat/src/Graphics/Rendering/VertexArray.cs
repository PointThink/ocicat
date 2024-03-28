namespace ocicat.Graphics.Rendering;

public enum ShaderDataType
{
	None = 0,
	Float, Float2, Float3, Float4, Int, Int2, Int3, Int4, Bool
}

public class BufferElement
{
	public ShaderDataType DataType;
	public string Name;
	public uint Offset;
	public uint Size;
	public uint Count;

	public BufferElement(string name, ShaderDataType type)
	{
		Name = name;
		DataType = type;
		Size = GetDataTypeSize(type);
		Offset = 0;
	}
	
	public static uint GetDataTypeSize( ShaderDataType type )
	{
		switch ( type )
		{
			case ShaderDataType.Float: return 4;
			case ShaderDataType.Float2: return 4 * 2;
			case ShaderDataType.Float3: return 4 * 3;
			case ShaderDataType.Float4: return 4 * 4;
			case ShaderDataType.Int: return 4;
			case ShaderDataType.Int2: return 4 * 2;
			case ShaderDataType.Int3: return 4 * 3;
			case ShaderDataType.Int4: return 4 * 4;
			case ShaderDataType.Bool: return 1;
		}

		return 0;
	}
	
	public static uint GetComponentCount(ShaderDataType dataType)
	{	
		switch ( dataType )
		{
			case ShaderDataType.Float: return 1;
			case ShaderDataType.Float2: return 2;
			case ShaderDataType.Float3: return 3;
			case ShaderDataType.Float4: return 4;
			case ShaderDataType.Int: return 1;
			case ShaderDataType.Int2: return 2;
			case ShaderDataType.Int3: return 3;
			case ShaderDataType.Int4: return 4;
			case ShaderDataType.Bool: return 1;
		}

		return 0;
	}

}

public class BufferLayout
{
	public BufferElement[] BufferElements { get; private set; }
	public uint Stride { get; private set; }
	
	public BufferLayout(BufferElement[] bufferElements)
	{
		BufferElements = bufferElements;
		CalculateOffsetAndStride();
	}

	private void CalculateOffsetAndStride()
	{
		uint offset = 0;
		Stride = 0;

		foreach ( BufferElement element in BufferElements )
		{
			element.Offset = offset;
			element.Count = BufferElement.GetComponentCount(element.DataType);
			offset += element.Size;
			Stride += element.Size;	
		}
	}
}

public abstract class VertexArray
{
	public abstract void Bind();
	public abstract void Unbind();
	
	public abstract void SetVertexBuffer(VertexBuffer vertexBuffer);
	public abstract void SetIndexBuffer(IndexBuffer indexBuffer);

	public abstract VertexBuffer GetVertexBuffer();
	public abstract IndexBuffer GetIndexBuffer();
	
	public static VertexArray Create(Renderer renderer)
	{
		switch (renderer.RenderingApi)
		{
			case RenderingApi.OpenGl:
				return new OpenGl.VertexArray();
		}

		throw new ArgumentException("Invalid RenderingApi");
	}
}