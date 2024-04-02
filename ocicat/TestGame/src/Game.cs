using System.Numerics;
using ocicat;
using ocicat.Audio;
using ocicat.Graphics;
using ocicat.Graphics.Rendering;
using ocicat.Input;
using Vector2 = ocicat.Vector2;

namespace TestGame;

public class InGame : GameState
{
	public override void Update()
	{
		if (Game.Window.IsKeyPressed(Key.Space))
		{
			Vector2 mousePos = Game.Window.GetMouseMotion();
			AudioHandle handle = Game.AudioEngine.PlaySound(Program.Sound);
			handle.Position = new Vector3(mousePos.X - 400, mousePos.Y - 300, 0);
			handle.Falloff = 0.01f;
			handle.Volume = 1;
		}
	}

	public override void Draw()
	{
		Game.Renderer.DrawCircle(new Vector2(400, 300), 30, 64, Color.Yellow);
		// Game.Renderer.DrawLine(new Vector2(400, 300), Game.Window.GetMouseMotion(), 1, Color.Blue);
	}
}

public static class Program
{
	public static Sound Sound;
	public static void Main(string[] args)
	{
		// Logging.LogLevel = LogLevel.Ocicat;
		
		Game.Create("Test game", 800, 600);
		Sound = new Sound(Game.AudioEngine, "erro.ogg");
		
		Game.ClearColor = Color.CreateFloat(0.2f, 0.2f, 0.2f, 1);
		Game.GameState = new InGame();

		Game.Run();
	}
}