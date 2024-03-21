using System.Numerics;
using ocicat;
using ocicat.Graphics;
using ocicat.Graphics.Rendering;
namespace TestGame;

class Player
{
	private Vector2 _position;
	private Vector2 _motion;
	
	public void Update(float deltaTime)
	{
		_motion.Y -= 800 * deltaTime;
		
		if ((Game.Window.IsKeyDown(Key.Space) || Game.Window.IsMouseButtonDown(0)) && _position.Y <= 0)
			_motion.Y += 600;

		if (Game.Window.IsKeyDown(Key.A))
			_motion.X = -400;
		if (Game.Window.IsKeyDown(Key.D))
			_motion.X = 400;
		
		_position.X += _motion.X * deltaTime;
		_position.Y += _motion.Y * deltaTime;

		_motion.X = 0;
		
		if (_position.Y < 0)
		{
			_motion.Y = 0;
			_position.Y = 0;
		}
	}

	public void Draw()
	{
		Game.Renderer.DrawRectTextured(_position, new Vector2(64, 64), Game.texture);
	}
}

class Game
{
	public static Window Window;
	public static Renderer Renderer;

	public static Texture texture;
	
	static void Main(string[] args)
	{
		Window = new Window("Hello", 1280, 720);

		Renderer = new Renderer(Window, RenderingApi.OpenGl);
		Renderer.RenderCommands.SetClearColor(0.2f, 0.2f, 0.2f, 1f);

		((OrthographicCamera)Renderer.Camera).Offset = new Vector2(0, 720 / 4);
		
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