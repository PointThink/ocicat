using ocicat.Graphics;
using ocicat.Graphics.Rendering;

namespace BasicWindow;

class Program
{
	static void Main(string[] args)
	{
		Window window = Window.Create("Hello world", 800, 600);
		Renderer renderer = new Renderer(window);
		
		while (!window.ShouldClose())
		{
			window.HandleEvents();
			renderer.BeginDrawing();
			
			renderer.ClearScreen(Color.CreateRGBA8(255, 255, 255, 255));
			
			renderer.EndDrawing();
			window.Present();
		}
	}
}