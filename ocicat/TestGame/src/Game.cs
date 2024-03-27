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

	public static AnimationTemplate AnimationTemplate;
	
	static void Main(string[] args)
	{
		Window = Window.Create("Hello", 1280, 720);
		
		Renderer = new Renderer(Window);
		Renderer.RenderCommands.SetClearColor(0.2f, 0.2f, 0.2f, 1f);

		AnimationTemplate = new AnimationTemplate([
			Texture.Create(Renderer, "image.jpg"),
			Texture.Create(Renderer, "image2.png")
		], 1);

		AnimationController animationController = new AnimationController(AnimationTemplate);

		CircleCollider collider = new CircleCollider(64);
		CircleCollider collider2 = new CircleCollider(128);

		collider2.Position = new Vector2(200, 200);
		
		while (!Window.ShouldClose())
		{
			Window.HandleEvents();
			Renderer.BeginDrawing();
			
			Renderer.RenderCommands.ClearScreen();
			
			animationController.Draw(Renderer, Window.GetMouseMotion(), new Vector2(64, 64));
			
			Renderer.EndDrawing();	
			Window.Present();
		}
	}
}