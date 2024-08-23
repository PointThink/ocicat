using ocicat.Graphics.Rendering;
using ocicat.Input;

namespace ocicat.Graphics;

public abstract class Window
{
	public float DeltaTime { get; protected set; }
	public float Time { get; protected set; }

	public int AASamples { get; protected set; }

	public Action<int, int>? OnResize = null;
	public Action<Key>? OnKeyPressed = null;
	public Action<int>? OnMousePressed = null;
	
	public RenderingApi RenderingApi { get; private set; }

	public abstract string Title { get; set; }
	public abstract Image Icon { get; set; }
	public abstract int Width { get; set; }
	public abstract int Height { get; set; }
	public abstract bool Resizable { get; set; }
	public abstract bool Fullscreen { get; set; }
	
	public abstract Vector2 MousePosition { get; set; }
	public abstract bool CursorVisible { get; set; }
	public abstract bool CursorLocked { get; set; }
	
	public abstract void HandleEvents();
	public abstract void Present();
	public abstract bool ShouldClose();

	public abstract bool IsKeyDown(Input.Key key);
	public abstract bool IsKeyPressed(Input.Key key);
	public abstract bool IsMouseButtonDown(int button);
	public abstract bool IsMouseButtonPressed(int button);

	/// <summary>
	/// Returns the current state of a gamepad
	/// </summary>
	/// <param name="gamepad">Gamepad ID. 0 is the first connected gamepad</param>
	/// <returns></returns>
	public abstract GamePadState GetGamePadState(int gamepad = 0);
	
	public static Window Create(string title, int width, int height, bool fullscreen = false, bool resizable = false, int samples = 0, RenderingApi? api = null)
	{
		if (api == null)
			api = RenderingApi.OpenGl;

		Window? window = null;
		
		switch (api)
		{
			case RenderingApi.OpenGl:
				// window = new OpenTKWindow(title, width, height, fullscreen, resizable, samples);
				window = new GLFWWindow(title, width, height, fullscreen, resizable, samples);
				break;
			default:
				throw new ArgumentException("Invalid RenderingApi");
		}

		window.RenderingApi = api.Value;

		return window;
	}
}