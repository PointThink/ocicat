using ocicat.Graphics;
using ocicat.Graphics.Rendering;

namespace ocicat.Physics;

/// <summary>
/// A base class for all colliders.
/// </summary>
public abstract class Collider
{
	public Vector2 Position;
	
	public abstract CollisionInfo TestCollision(Collider collider);
	public abstract CollisionInfo TestCollision(RectCollider collider);
	public abstract CollisionInfo TestCollision(CircleCollider collider);

	public abstract void DebugDraw(Renderer renderer, Color color);
}

public class RectCollider : Collider
{
	public Vector2 Size;

	public RectCollider(Vector2 size)
	{
		Size = size;
	}

	public override CollisionInfo TestCollision(Collider collider)
	{
		return collider.TestCollision(this);
	}

	public override CollisionInfo TestCollision(RectCollider collider)
	{
		return CollisionAlgo.TestCollisionRectRect(this, collider);
	}

	public override CollisionInfo TestCollision(CircleCollider collider)
	{
		throw new NotImplementedException();
	}

	public override void DebugDraw(Renderer renderer, Color color)
	{
		renderer.DrawRect(Position, Size, color);
	}
}

public class CircleCollider : Collider
{
	public float Radius;

	public CircleCollider(float radius)
	{
		Radius = radius;
	}

	public override CollisionInfo TestCollision(Collider collider)
	{
		return collider.TestCollision(this);
	}

	public override CollisionInfo TestCollision(RectCollider collider)
	{
		throw new NotImplementedException();
	}

	public override CollisionInfo TestCollision(CircleCollider collider)
	{
		return CollisionAlgo.TestCollisionCircleVCircle(this, collider);
	}

	public override void DebugDraw(Renderer renderer, Color color)
	{
	}
}