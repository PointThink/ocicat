using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace ocicat.Graphics;

public class Window
{
	private GameWindow _tkWindow;
	
	public float DeltaTime { get; private set; }
	private DateTime frameBeginTime;
	
	public Window(string title, int width, int height, bool fullscreen = false, bool resizable = false)
	{
		NativeWindowSettings nativeWindowSettings = NativeWindowSettings.Default;
		GameWindowSettings gameWindowSettings = new GameWindowSettings();

		nativeWindowSettings.Title = title;
		nativeWindowSettings.ClientSize = new Vector2i(width, height);
		
		if (!resizable)
		{
			nativeWindowSettings.MinimumClientSize = new Vector2i(width, height);
			nativeWindowSettings.MaximumClientSize = new Vector2i(width, height);
		}
		
		if (fullscreen)
			nativeWindowSettings.WindowState = WindowState.Fullscreen;
		
		_tkWindow = new GameWindow(gameWindowSettings, nativeWindowSettings);
		
		DeltaTime = 0;
	}
	
	public void HandleEvents()
	{
		frameBeginTime = DateTime.Now;
		_tkWindow.ProcessEvents(0);
	}

	public void Present()
	{
		_tkWindow.SwapBuffers();
		DeltaTime = ( (float) (DateTime.Now - frameBeginTime).TotalMilliseconds ) / 1000;
	}
	
	public bool ShouldClose()
	{
		return _tkWindow.IsExiting;
	}

	public bool IsKeyDown(Key key)
	{
		return _tkWindow.IsKeyDown((Keys) key);
	}
	
	public bool IsKeyPressed(Key key)
	{
		return _tkWindow.IsKeyPressed((Keys) key);
	}
	
	public bool IsMouseButtonDown(int button)
	{
		return _tkWindow.IsMouseButtonDown(MouseButton.Button1 + button);
	}
	
	public bool IsMouseButtonPressed(int button)
	{
		return _tkWindow.IsMouseButtonPressed(MouseButton.Button1 + button);
	}
}