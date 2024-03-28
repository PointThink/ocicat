using ocicat;
using ocicat.Graphics;
using ocicat.Graphics.Rendering;

namespace Animation;

class Program
{
	static void Main(string[] args)
	{
		Window window = Window.Create("Hello world", 800, 600);
		Renderer renderer = new Renderer(window);

		// created once per animation
		AnimationTemplate template = new AnimationTemplate([
			Texture.Create(renderer, "image.jpg"),
			Texture.Create(renderer, "image2.png"),
		], 1);

		// create once per object that uses the animation
		AnimationController animationController = new AnimationController(template);
		
		while (!window.ShouldClose())
		{
			window.HandleEvents();
			renderer.BeginDrawing();
			
			renderer.ClearScreen(Color.CreateRGBA8(255, 255, 255, 255));
			animationController.Draw(renderer, new Vector2(0, 0), new Vector2(256, 256));
			
			renderer.EndDrawing();
			window.Present();
		}
	}
}