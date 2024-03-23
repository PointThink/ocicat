using System.Numerics;
using ocicat.Graphics.Rendering;

namespace ocicat.Graphics;

public abstract class Window
{
	public float DeltaTime { get; protected set; }
	
	public RenderingApi RenderingApi { get; private set; }

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
	
	public static Window? Create(string title, int width, int height, bool fullscreen = false, bool resizable = false,  RenderingApi? api = null)
	{
		if (api == null)
			api = RenderingApi.OpenGl;

		Window? window = null;
		
		switch (api)
		{
			case RenderingApi.OpenGl:
				window = new OpenTKWindow(title, width, height, fullscreen, resizable);
				break;
		}

		window.RenderingApi = api.Value;

		return window;
	}
}