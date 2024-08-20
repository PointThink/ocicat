using OpenTK.Mathematics;

namespace ocicat.Graphics.Rendering;

public abstract class Camera
{
	public abstract Matrix4 CalculateProjection();
	public abstract  Matrix4 CalculateView();
}

public class OrthographicCamera : Camera
{
	public float Width;
	public float Height;

	public Vector2 Offset = new Vector2(0, 0);
	
	public OrthographicCamera(float width, float height)
	{
		Width = width;
		Height = height;
	}

	public override Matrix4 CalculateProjection()
	{
		return Matrix4.CreateOrthographicOffCenter(0, Width, Height, 0, 0f, 1f);
	}

	public override Matrix4 CalculateView()
	{
		return Matrix4.CreateTranslation(-Offset.X, -Offset.Y, 0);
	}
}
