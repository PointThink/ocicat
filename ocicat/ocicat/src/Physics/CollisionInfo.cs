using System.Numerics;

namespace ocicat.Physics;

public struct CollisionInfo
{
	public bool HasCollision;
	public Vector2 PointA;
	public Vector2 PointB;
	public Vector2 Normal;
	public float Depth;
}