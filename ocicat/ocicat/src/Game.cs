using ocicat.Audio;
using ocicat.Graphics;
using ocicat.Graphics.Rendering;
using ocicat.Input;

namespace ocicat;

/// <summary>
/// Abstracts all the low level functionality into an easy to use API.
/// </summary>
public static class Game
{
	public static Window Window { get; private set; } = null!;
	public static Renderer Renderer { get; private set; } = null!;
	public static AudioEngine AudioEngine { get; private set; } = null!;
	public static Bindings Bindings { get; private set; } = null!;

	public static double Tickrate = 64;
	private static double _nextTickTime;
	
	/// <summary>
	/// Color used to clear the screen by the renderer
	/// </summary>
	public static Color ClearColor = Color.CreateFloat(0, 0, 0, 1);
	
	/// <summary>
	/// The current game state
	/// </summary>
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

	/// <summary>
	/// Initializes the game.
	/// </summary>
	/// <param name="title">Window title</param>
	/// <param name="width">Window width</param>
	/// <param name="height">Window height</param>
	public static void Create(string title, int width, int height)
	{
		Window = Window.Create(title, width, height);
		Renderer = new Renderer(Window);
		AudioEngine = new AudioEngine();
		Bindings = new Bindings(Window);
	}
	
	/// <summary>
	/// Starts the game loop.
	/// </summary>
	public static void Run()
	{
		while (!Window.ShouldClose())
		{
			Window.HandleEvents();
			
			AudioEngine.CleanFinishedSounds();
			
			if (_gameState != null)
			{
				if (Window.Time >= _nextTickTime)
				{
					_gameState.Tick();
					_nextTickTime = Window.Time + 1 / Tickrate;
				}
				
				_gameState.Update();
			}
			
			Renderer.BeginDrawing();
			Renderer.ClearScreen(ClearColor);
			
			if (_gameState != null)
				_gameState.Draw();
			
			Renderer.EndDrawing();
			Window.Present();
		}
	}
}