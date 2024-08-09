using System.Numerics;
using ocicat;
using ocicat.Audio;
using ocicat.Graphics;
using ocicat.Input;
using Vector2 = ocicat.Vector2;

namespace TestGame;

public class InGame : GameState
{
	double _lastSoundTime = 0.0;
	private AudioHandle? _handle = null;

	public InGame()
	{
		Vector2 mousePos = Game.Window.MousePosition;
		
		_handle = Game.AudioEngine.CreateHandle(Program.Sound);
		_handle.ManualCleanup = true;
		_handle.Position = Game.AudioEngine.ListenerPosition;
		_handle.Falloff = 0.01f;
		_handle.Volume = 1;
				
		_lastSoundTime = Game.Window.Time;
	}
	~InGame()
	{
		_handle.Destroy();
	}
	
	public override void Update()
	{
		if (_lastSoundTime + 0.075 < Game.Window.Time)
		{
			if (Game.Window.IsKeyDown(Key.Space))
			{
				_handle.Play();
				_lastSoundTime = Game.Window.Time;
			}
		}
	}

	public override void Tick()
	{
		Console.WriteLine($"{1 / Game.Window.DeltaTime} fps");
	}

	public override void Draw()
	{
		Game.Renderer.DrawCircle(new Vector2(Game.Renderer.Width / 2, Game.Renderer.Height / 2), 30, 64, Color.Blue);
		Game.Renderer.DrawLine(new Vector2(Game.Renderer.Width / 2, Game.Renderer.Height / 2), Game.Window.MousePosition, 3, Color.Red);
		// Game.Renderer.DrawRectLines(new Vector2(40, 40), new Vector2(128, 128), Color.Cyan, 5);
	}
}

public static class Program
{
	public static Sound Sound;
	
	public static void Main(string[] args)
	{
		Game.Create("Test game", 800, 600, false, true);
		Game.Window.Icon = new Image("image.jpg");
		
		Sound = new Sound(Game.AudioEngine, "erro.ogg");
		
		Game.Window.Title = "Hello world";
		Game.Window.Width = 1024;
		Game.Window.Height = 768;
		Game.Window.CursorVisible = false;
		
		Game.ClearColor = Color.CreateFloat(0.2f, 0.2f, 0.2f, 1);
		Game.GameState = new InGame();
		Game.Run();
	}
}