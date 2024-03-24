using System.Numerics;
using ocicat;
using ocicat.Graphics;
using ocicat.Graphics.Rendering;
using ocicat.Input;
using ocicat.Physics;
using Vector2 = ocicat.Vector2;

namespace TestGame;

class Game
{
	public static Window Window;
	public static Renderer Renderer;
	public static Bindings Bindings;

	public static AnimationTemplate AnimationTemplate;
	public static Font Font;
	
	static void Main(string[] args)
	{
		Window = Window.Create("Hello", 1280, 720);
		
		Window.CursorVisible = false;
		
		Renderer = new Renderer(Window);
		Renderer.RenderCommands.SetClearColor(0.2f, 0.2f, 0.2f, 1f);

		Bindings = new Bindings(Window);
		
		Bindings.AddBinding("jump", new KeyboardBind(Key.Space));

		AnimationTemplate = new AnimationTemplate([
			Texture.Create(Renderer, "image.jpg"),
			Texture.Create(Renderer, "image2.png")
		], 1);
		
		Font = new Font(Renderer, "Roboto-Regular.ttf", 32);

		RNG rng = new RNG();

		RectCollider rCollider = new RectCollider(new Vector2(64, 64));
		RectCollider rCollider2 = new RectCollider(new Vector2(128, 256));

		rCollider2.Position = new Vector2(200, 200);
		
		while (!Window.ShouldClose())
		{
			Window.HandleEvents();

			rCollider.Position = Window.GetMouseMotion();
			
			Renderer.RenderCommands.ClearScreen();
			
			rCollider.DebugDraw(Renderer, Color.CreateFloat(1, 0, 0, 0.5f));
			rCollider2.DebugDraw(Renderer, Color.CreateFloat(0, 0, 1, 0.5f));

			CollisionInfo collisionInfo = rCollider.TestCollision(rCollider2);

			if (collisionInfo.HasCollision)
			{
				Vector2 depthV = new Vector2(collisionInfo.Depth, collisionInfo.Depth);
				Renderer.DrawRect(rCollider.Position - collisionInfo.Normal * depthV, new Vector2(64, 64),
					Color.CreateFloat(0, 1, 0, 0.5f));
			}
			
			Window.Present();
		}
	}
}