using ocicat;
using ocicat.Audio;
using ocicat.Input;

namespace Audio;

class Game : Application
{
	public Sound Sound;

	public Game() : base("Press space to play sound", 800, 600) { }

	public override void Initialize()
	{
		Sound = new Sound(AudioEngine, "erro.ogg");
	}
	
	public override void Update(float deltaTime)
	{
		if (Window.IsKeyPressed(Key.Space))
		{
			AudioEngine.PlaySound(Sound);
		}
	}
	
	static void Main(string[] args)
	{
		new Game().Run();
	}
}