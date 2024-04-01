using ocicat.Audio;
using ocicat.Graphics;
using ocicat.Graphics.Rendering;

namespace ocicat;

public static class Game
{
	public static Window Window { get; private set; }
	public static Renderer Renderer { get; private set; }
	public static AudioEngine AudioEngine { get; private set; }

	public static Color ClearColor = Color.CreateFloat(0, 0, 0, 1);

	private static GameState? _gameState = null;
	
	public static GameState? GameState
	{
		get => _gameState;
		set
		{
			if (_gameState != null)
				_gameState.OnLeave();

			_gameState = value;
			
			if (_gameState != null)
				_gameState.OnEnter();
		}
	}

	public static void Create(string title, int width, int height)
	{
		Window = Window.Create(title, width, height);
		Renderer = new Renderer(Window);
		AudioEngine = new AudioEngine();
	}

	public static void Run()
	{
		while (!Window.ShouldClose())
		{
			Window.HandleEvents();
			
			AudioEngine.CleanFinishedSounds();
			
			if (_gameState != null)
				_gameState.Update();
			
			Renderer.BeginDrawing();
			Renderer.ClearScreen(ClearColor);
			
			if (_gameState != null)
				_gameState.Draw();
			
			Renderer.EndDrawing();
			Window.Present();
		}
	}
}