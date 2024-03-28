using ocicat;
using ocicat.Graphics.Rendering;
using ocicat.Graphics;

namespace Textures;

class Program
{
	static void Main(string[] args)
	{
		Window window = Window.Create("Hello world", 800, 600);
		Renderer renderer = new Renderer(window);
		
		Texture texture = Texture.Create(renderer, "image.jpg");
		
		while (!window.ShouldClose())
		{
			window.HandleEvents();
			renderer.BeginDrawing();
			
			renderer.ClearScreen(Color.CreateRGBA8(255, 255, 255, 255));
			
			renderer.DrawRectTextured(
				new Vector2(0, 0), 
				new Vector2(texture.GetWidth(), texture.GetWidth()),
				texture
			);
			
			renderer.EndDrawing();
			window.Present();
		}
	}
}