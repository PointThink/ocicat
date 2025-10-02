namespace ocicat.Physics;

public class CollisionAlgo
{
	public static CollisionInfo TestCollisionRectRect(RectCollider rect1, RectCollider rect2)
	{
		// I have no idea what any of this does
		CollisionInfo collisionInfo = new CollisionInfo();

		Vector2 aCenter = rect1.Position;
		Vector2 bCenter = rect2.Position;

		Vector2 n = aCenter - bCenter;

		float aExtent = ((rect1.Position.X + rect1.Size.X) - rect1.Position.X) / 2;
		float bExtent = ((rect2.Position.X + rect2.Size.X) - rect2.Position.X) / 2;

		float xOverlap = aExtent + bExtent - float.Abs(n.X);

		if (xOverlap > 0)
		{
			aExtent = ((rect1.Position.Y + rect1.Size.Y) - rect1.Position.Y) / 2;
			bExtent = ((rect2.Position.Y + rect2.Size.Y) - rect2.Position.Y) / 2;

			float yOverlap = aExtent + bExtent - float.Abs(n.Y);

			if (yOverlap > 0)
			{
				if (xOverlap > yOverlap)
				{
					if (n.Y < 0)
						collisionInfo.Normal = new Vector2(0, 1);
					else 
						collisionInfo.Normal = new Vector2(0, -1);

					collisionInfo.Depth = yOverlap;
					collisionInfo.HasCollision = true;
				}
				else
				{
					if (n.X < 0)
						collisionInfo.Normal = new Vector2(1, 0);
					else
						collisionInfo.Normal = new Vector2(-1, 0);

					collisionInfo.HasCollision = true;
					collisionInfo.Depth = xOverlap;
				}
			}
		}
		
		return collisionInfo;
	}

	public static CollisionInfo TestCollisionCircleVCircle(CircleCollider circle1, CircleCollider circle2)
	{
		CollisionInfo collisionInfo = new CollisionInfo();
		float distance = Vector2.GetDistance(circle1.Position, circle2.Position);

		if (distance <= circle1.Radius + circle2.Radius)
		{
			collisionInfo.HasCollision = true;
			collisionInfo.Depth = distance - (circle1.Radius + circle2.Radius);
			collisionInfo.Normal = Vector2.FromDegrees(Vector2.GetDirection(circle1.Position, circle2.Position));
		}

		return collisionInfo;
	}
}