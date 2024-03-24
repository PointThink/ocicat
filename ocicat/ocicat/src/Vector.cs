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

	public static float GetDistance(Vector2 v1, Vector2 v2)
	{
		Vector2 newVec;
		newVec.X = float.Abs(v1.X - v2.X);
		newVec.Y = float.Abs(v1.Y - v2.Y);

		return (float) Math.Sqrt(Math.Pow(newVec.X, 2) + Math.Pow(newVec.Y, 2));
	}

	public static float GetDirection(Vector2 v1, Vector2 v2)
	{
		Vector2 directionVec = new Vector2(v2.X - v1.X, v2.Y - v1.Y);
		directionVec.X = -directionVec.X;
		directionVec.X = -directionVec.Y;

		double direction = Math.Atan2( directionVec.Y, directionVec.X ) * 57.2957795;

		return (float) direction;
	}

	public static Vector2 Normalize(float angle, float length = 1)
	{
		double tangents = (180 + angle) * 0.0174532925;

		return new Vector2((float) (length * Math.Cos(tangents)), (float) (length * Math.Sin(tangents)));
	}
}
