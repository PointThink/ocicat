using System.Numerics;
using ocicat.Graphics;
using ocicat.Graphics.Rendering;
using Color = ocicat.Graphics.Color;

namespace TestGame;

class Program
{
	static void Main(string[] args)
	{
		Window window = new Window("Hello", 800, 600);
		
		Renderer renderer = new Renderer(RenderingApi.OpenGl);
		
		renderer.RenderCommands.SetClearColor(0.2f, 0.2f, 0.2f, 1f);
		
		while (!window.ShouldClose())
		{
			window.HandleEvents();
			
			renderer.RenderCommands.ClearScreen();
			renderer.DrawRect(new Vector2(20, 20), new Vector2(40, 40), Color.CreateRGBA8(255, 0, 255, 255));
			
			window.SwapBuffers();
		}
	}
}