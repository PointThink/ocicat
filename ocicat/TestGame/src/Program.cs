using System.Numerics;
using ocicat.Graphics;
using ocicat.Graphics.Rendering;
using Color = ocicat.Graphics.Color;

namespace TestGame;

class Player
{
	
}

class Program
{
	static void Main(string[] args)
	{
		Window window = new Window("Hello", 800, 600);
		
		Renderer renderer = new Renderer(RenderingApi.OpenGl);
		renderer.RenderCommands.SetClearColor(0.2f, 0.2f, 0.2f, 1f);
		
		Texture texture = Texture.Create(renderer, "image.jpg");
		
		while (!window.ShouldClose())
		{
			window.HandleEvents();
			
			renderer.RenderCommands.ClearScreen();
			renderer.DrawRectTextured(new Vector2(20, 20), new Vector2(64, 64), texture);
			
			window.SwapBuffers();
		}
	}
}