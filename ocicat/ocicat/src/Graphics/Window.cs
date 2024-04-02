using System.Data.SqlTypes;
using System.Numerics;
using ocicat.Graphics.Rendering;
using OpenTK.Graphics.OpenGL4;

namespace ocicat.Graphics;

public abstract class Window
{
	public float DeltaTime { get; protected set; }
	public float Time { get; protected set; }

	public int AASamples { get; protected set; }
	
	public Action<int, int>? OnResize = null;
	
	public RenderingApi RenderingApi { get; private set; }

	public abstract bool CursorVisible { get; set; }
	public abstract bool CursorLocked { get; set; }
	
	public int Width { get; protected set; }
	public int Height { get; protected set; }
	
	public abstract void HandleEvents();
	public abstract void Present();
	public abstract bool ShouldClose();

	public abstract bool IsKeyDown(Input.Key key);
	public abstract bool IsKeyPressed(Input.Key key);
	public abstract bool IsMouseButtonDown(int button);
	public abstract bool IsMouseButtonPressed(int button);

	public abstract Vector2 GetMouseMotion();
	
	public static Window Create(string title, int width, int height, bool fullscreen = false, bool resizable = false, int samples = 0, RenderingApi? api = null)
	{
		if (api == null)
			api = RenderingApi.OpenGl;

		Window? window = null;
		
		switch (api)
		{
			case RenderingApi.OpenGl:
				window = new OpenTKWindow(title, width, height, fullscreen, resizable, samples);
				break;
			default:
				throw new ArgumentException("Invalid RenderingApi");
		}

		window.RenderingApi = api.Value;

		return window;
	}
}