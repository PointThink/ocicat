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

		Collider collider = new CircleCollider(64);
		Collider collider2 = new CircleCollider(128);

		collider2.Position = new Vector2(200, 200);
		
		while (!Window.ShouldClose())
		{
			Window.HandleEvents();

			collider.Position = Window.GetMouseMotion();
			
			Renderer.RenderCommands.ClearScreen();
			
			collider.DebugDraw(Renderer, Color.CreateFloat(1, 0, 0, 0.5f));
			collider2.DebugDraw(Renderer, Color.CreateFloat(0, 0, 1, 0.5f));

			CollisionInfo collisionInfo = collider.TestCollision(collider2);

			if (collisionInfo.HasCollision)
			{
				
			}
			
			Window.Present();
		}
	}
}