using System.Diagnostics;
using System.Numerics;
using ocicat;
using ocicat.Graphics;
using ocicat.Graphics.Rendering;
using Color = ocicat.Graphics.Color;

namespace TestGame;

class Player
{
	Texture texture = Texture.Create(Program.Renderer, "image.jpg");

	private Vector2 position;
	private Vector2 motion = new Vector2(20, 20);
	
	public void Update(float deltaTime)
	{
		position.X += motion.X * deltaTime;
		position.Y += motion.Y * deltaTime;
		
		// Logging.Log(LogLevel.Info, $"{deltaTime}");
	}

	public void Draw()
	{
		Program.Renderer.DrawRectTextured(position, new Vector2(64, 64), texture);
	}
}

class Program
{
	public static Window Window;
	public static Renderer Renderer;
	
	static void Main(string[] args)
	{
		Window = new Window("Hello", 1024, 768);

		Renderer = new Renderer(RenderingApi.OpenGl);
		Renderer.RenderCommands.SetClearColor(0.2f, 0.2f, 0.2f, 1f);

		Player player = new Player();
		
		while (!Window.ShouldClose())
		{
			Window.HandleEvents();
			
			player.Update(Window.DeltaTime);
			
			Renderer.RenderCommands.ClearScreen();
			player.Draw();
			
			Window.Present();
		}
	}
}