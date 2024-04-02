using System.Runtime.InteropServices;
using OpenTK.Graphics.OpenGL4;

namespace ocicat.Graphics.Rendering.OpenGl;

public class RenderCommands : Rendering.RenderCommands
{
	public override void Init()
	{
		// GL.DebugMessageCallback(OnDebugMessage, IntPtr.Zero);
		// GL.Enable(EnableCap.DebugOutput);
		
		GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
		GL.Enable(EnableCap.Blend);
		
		GL.PixelStore(PixelStoreParameter.UnpackAlignment, 1);

		Logging.Log(LogLevel.Info, $"Vendor: {GL.GetString(StringName.Vendor)}");
        Logging.Log(LogLevel.Info, $"GL Version: {GL.GetString(StringName.Version)}");
        Logging.Log(LogLevel.Info, $"Renderer: {GL.GetString(StringName.Renderer)}");
        Logging.Log(LogLevel.Info, $"GLSL Version: {GL.GetString(StringName.ShadingLanguageVersion)}");
        
        GL.Enable(EnableCap.Multisample);
    }

	public override void SetClearColor(float r, float g, float b, float a)
	{
		GL.ClearColor(r, g, b, a);
	}

	public override void ClearScreen()
	{
		GL.Clear(ClearBufferMask.ColorBufferBit);
	}

	public override void DrawArrays(Rendering.VertexArray vertexArray, int count)
	{
		vertexArray.Bind();
		GL.DrawArrays(PrimitiveType.Triangles, 0, count);
	}

	public override void DrawIndexed(Rendering.VertexArray vertexArray)
	{
		vertexArray.Bind();
		vertexArray.GetVertexBuffer().Bind();
		vertexArray.GetIndexBuffer().Bind();
		GL.DrawElements(PrimitiveType.Triangles, (int) vertexArray.GetIndexBuffer().GetIndexCount(), DrawElementsType.UnsignedInt, 0);		
	}
	
	// This is copied from OpenTK docs.
	// Ignore the comments
	private static void OnDebugMessage(
		DebugSource source,     // Source of the debugging message.
		DebugType type,         // Type of the debugging message.
		int id,                 // ID associated with the message.
		DebugSeverity severity, // Severity of the message.
		int length,             // Length of the string in pMessage.
		IntPtr pMessage,        // Pointer to message string.
		IntPtr pUserParam)      // The pointer you gave to OpenGL, explained later.
	{
		// In order to access the string pointed to by pMessage, you can use Marshal
		// class to copy its contents to a C# string without unsafe code. You can
		// also use the new function Marshal.PtrToStringUTF8 since .NET Core 1.1.
		string message = Marshal.PtrToStringAnsi(pMessage, length);

		// The rest of the function is up to you to implement, however a debug output
		// is always useful.
		Console.WriteLine("[{0} source={1} type={2} id={3}] {4}", severity, source, type, id, message);

		// Potentially, you may want to throw from the function for certain severity
		// messages.
		if (type == DebugType.DebugTypeError)
		{
			throw new Exception(message);
		}
	}
}