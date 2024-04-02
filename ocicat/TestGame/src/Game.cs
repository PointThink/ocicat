using ocicat;
using ocicat.Audio;
using ocicat.Graphics;
using ocicat.Input;

namespace TestGame;

public class InGame : GameState
{
	public override void Update()
	{
		if (Game.Window.IsKeyPressed(Key.Space))
		{
			AudioHandle handle = Game.AudioEngine.PlaySound(Program.Sound);
			handle.Pan = 0;
		}
	}

	public override void Draw()
	{
		// Game.Renderer.DrawCircle(new Vector2(400, 300), 128, 64, Color.Yellow);
		Game.Renderer.DrawLine(new Vector2(0, 0), Game.Window.GetMouseMotion(), 10, Color.Blue);
	}
}

public static class Program
{
	public static Sound Sound;
	public static void Main(string[] args)
	{
		Game.Create("Test game", 800, 600);
		Sound = new Sound(Game.AudioEngine, "erro.ogg");
		
		Game.ClearColor = Color.CreateFloat(0.2f, 0.2f, 0.2f, 1);
		Game.GameState = new InGame();
		
		Game.Run();
	}
}