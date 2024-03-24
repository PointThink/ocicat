namespace ocicat;

using System.Numerics;

public struct Vector2
{
	public float X;
	public float Y;

	public Vector2()
	{
		X = 0;
		Y = 0;
	}
	
	public Vector2(float x, float y)
	{
		X = x;
		Y = y;
	}
	
	public static Vector2 operator +(Vector2 a, Vector2 b)
	{
		return new Vector2(a.X + b.X, a.Y + b.Y);
	}
	
	public static Vector2 operator -(Vector2 a, Vector2 b)
	{
		return new Vector2(a.X - b.X, a.Y - b.Y);
	}
	
	public static Vector2 operator *(Vector2 a, Vector2 b)
	{
		return new Vector2(a.X * b.X, a.Y * b.Y);
	}
	
	public static Vector2 operator /(Vector2 a, Vector2 b)
	{
		return new Vector2(a.X / b.X, a.Y / b.Y);
	}
}
