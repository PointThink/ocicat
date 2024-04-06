using ocicat;
using ocicat.Audio;
using ocicat.Input;

namespace Audio;

class InGame : GameState
{
	public override void Update()
	{
		if (Game.Window.IsKeyPressed(Key.Space))
		{
			Game.AudioEngine.PlaySound(Program.Sound);
		}
	}
}

class Program
{
	public static Sound Sound;
	
	static void Main(string[] args)
	{
		Game.Create("Press space to play sound", 800, 600);

		Sound = new Sound(Game.AudioEngine, "erro.ogg");
		
		Game.GameState = new InGame();
		Game.Run();
	}
}