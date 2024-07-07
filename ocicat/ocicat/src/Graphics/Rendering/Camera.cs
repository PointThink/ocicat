using OpenTK.Mathematics;

namespace ocicat.Graphics.Rendering;

public abstract class Camera
{
	public abstract Matrix4 CalculateProjection();
	public abstract  Matrix4 CalculateView();
}

public class OrthographicCamera : Camera
{
	public int Width { get; private set; }
	public int Height { get; private set; }

	public Vector2 Offset = new Vector2(0, 0);
	
	public OrthographicCamera(int width, int height)
	{
		Width = width;
		Height = height;
	}

	public override Matrix4 CalculateProjection()
	{
		return Matrix4.CreateOrthographicOffCenter(Offset.X, Width + Offset.X, Offset.Y, Height + Offset.Y, 0f, 1f);
	}

	public override Matrix4 CalculateView()
	{
		return Matrix4.CreateTranslation(0, 0, 0);
	}
}
