using ocicat;
using ocicat.Graphics;
using ocicat.Graphics.Rendering;

namespace Text;

class Program {
	static void Main(string[] args)
	{
		Window window = Window.Create("Hello world", 800, 450);
		Renderer renderer = new Renderer(window);

		Font font = new Font(renderer, "OpenSans.ttf", 24);
		
		while (!window.ShouldClose())
		{
			window.HandleEvents();
			renderer.BeginDrawing();
			
			renderer.ClearScreen(Color.CreateRGBA8(25, 25, 25, 255));
			renderer.DrawText("The quick brown fox jumps over the lazy dog 1234567890", font, new Vector2(30c, 30), Color.CreateFloat(1, 1, 1, 1), 1f);
			
			renderer.EndDrawing();
			window.Present();
		}
	}
}