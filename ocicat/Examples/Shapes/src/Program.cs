using ocicat;
using ocicat.Graphics;
using ocicat.Graphics.Rendering;

namespace Shapes;

class Program
{
	static void Main(string[] args)
	{
		Window window = Window.Create("Hello world", 800, 600, false, false, 82);
		Renderer renderer = new Renderer(window);
		
		Font font = new Font(renderer, "OpenSans.ttf", 24);

		float fps = 0;
		double nextFpsUpdate = window.Time;
		
		while (!window.ShouldClose())
		{
			window.HandleEvents();
			renderer.BeginDrawing();
			
			renderer.ClearScreen(Color.CreateRGBA8(25, 25, 25, 25));
			
			renderer.DrawRect(new Vector2(200, 200), new Vector2(200, 150), Color.CreateFloat(0.2f, 0.2f, 1, 1));
			renderer.DrawCircle(new Vector2(600, 100), 64, 32, Color.CreateFloat(1f, 1, 0.2f, 1));
			renderer.DrawTriangle(new Vector2(50, 50), new Vector2(100, 150), new Vector2(150, 50), Color.CreateFloat(1, 0.2f, 0.2f, 1));
			renderer.DrawRoundedRect(new Vector2(600, 300), new Vector2(100, 150), 30, Color.CreateFloat(1, 0.2f, 1, 1));

			if (window.Time > nextFpsUpdate)
			{
				nextFpsUpdate = window.Time + 1;
				fps = 1f / window.DeltaTime;
			}
			
			renderer.DrawText($"FPS: {fps}", font, new Vector2(20, 20));
			
			renderer.EndDrawing();
			window.Present();
		}
	}
}