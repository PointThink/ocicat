using System.Numerics;
using ocicat;
using ocicat.Graphics;
using ocicat.Graphics.Rendering;
namespace TestGame;

class Player
{
	private Vector2 position;
	private Vector2 motion;
	
	public void Update(float deltaTime)
	{
		motion.Y -= 800 * deltaTime;
		
		if (Game.Window.IsKeyDown(Key.Space) && position.Y <= 0)
			motion.Y += 600;

		if (Game.Window.IsKeyDown(Key.A))
			motion.X = -400;
		if (Game.Window.IsKeyDown(Key.D))
			motion.X = 400;
		
		position.X += motion.X * deltaTime;
		position.Y += motion.Y * deltaTime;

		if (position.Y < 0)
		{
			motion.Y = 0;
			motion.X = 0;
			position.Y = 0;
		}
	}

	public void Draw()
	{
		Game.Renderer.DrawRectTextured(position, new Vector2(64, 64), Game.texture);
	}
}

class Game
{
	public static Window Window;
	public static Renderer Renderer;

	public static Texture texture;
	
	static void Main(string[] args)
	{
		Window = new Window("Hello", 1024, 768);

		Renderer = new Renderer(RenderingApi.OpenGl);
		Renderer.RenderCommands.SetClearColor(0.2f, 0.2f, 0.2f, 1f);
		
		texture = Texture.Create(Game.Renderer, "image.jpg");
		
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