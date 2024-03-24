using System.Numerics;
using ocicat;
using ocicat.Graphics;
using ocicat.Graphics.Rendering;
using ocicat.Input;

namespace TestGame;

class Player
{
	private Vector2 _position;
	private Vector2 _motion;

	private AnimationController _animation;

	public Player()
	{
		_animation = new AnimationController(Game.AnimationTemplate);
	}
	
	public void Update(float deltaTime)
	{
		_motion.Y -= 800 * deltaTime;
		
		if (Game.Bindings.IsPressed("jump") && _position.Y <= 0)
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
		_animation.Draw(Game.Renderer, _position, new Vector2(64, 64));
	}
}

class Game
{
	public static Window Window;
	public static Renderer Renderer;
	public static Bindings Bindings;

	public static AnimationTemplate AnimationTemplate;
	public static Font Font;
	
	static void Main(string[] args)
	{
		Window = Window.Create("Hello", 1280, 720);

		Renderer = new Renderer(Window);
		Renderer.RenderCommands.SetClearColor(0.2f, 0.2f, 0.2f, 1f);

		Bindings = new Bindings(Window);
		
		Bindings.AddBinding("jump", new KeyboardBind(Key.Space));

		AnimationTemplate = new AnimationTemplate([
			Texture.Create(Renderer, "image.jpg"),
			Texture.Create(Renderer, "image2.png")
		], 1);
		
		Font = new Font(Renderer, "Roboto-Regular.ttf", 64);
		
		Player player = new Player();
		
		while (!Window.ShouldClose())
		{
			Window.HandleEvents();
			
			player.Update(Window.DeltaTime);
			
			Renderer.RenderCommands.ClearScreen();
			player.Draw();
			Renderer.DrawRect(Window.GetMouseMotion(), new Vector2(3, 3), Color.CreateFloat(1, 1, 1, 1));
			Renderer.DrawText("Hello world", Font, new Vector2(100, 100));
			Window.Present();
		}
	}
}