using System.Numerics;

namespace ocicat.Physics;

/// <summary>
/// The result of a collision check
/// </summary>
public struct CollisionInfo
{
	public bool HasCollision;
	public Vector2 Normal;
	public float Depth;
}