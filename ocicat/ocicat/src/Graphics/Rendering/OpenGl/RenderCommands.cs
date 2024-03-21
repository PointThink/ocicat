using OpenTK.Graphics.ES11;

namespace ocicat.Graphics.Rendering.OpenGl;

public class RenderCommands : Rendering.RenderCommands
{
	public override void SetClearColor(float r, float g, float b, float a)
	{
		GL.ClearColor(r, g, b, a);
	}

	public override void ClearScreen()
	{
		GL.Clear(ClearBufferMask.ColorBufferBit);
	}

	public override void DrawIndexed(Rendering.VertexArray vertexArray)
	{
		vertexArray.Bind();
		GL.DrawElements(PrimitiveType.Triangles, (int) vertexArray.GetIndexBuffer().GetIndexCount(), DrawElementsType.UnsignedInt, 0);		
	}
}