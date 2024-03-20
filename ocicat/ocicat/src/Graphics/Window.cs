using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace ocicat.Graphics;

public class Window
{
	private GameWindow _tkWindow;
	
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
	}

	public void HandleEvents()
	{
		_tkWindow.ProcessEvents(0);
	}

	public void SwapBuffers()
	{
		_tkWindow.SwapBuffers();
	}
	
	public bool ShouldClose()
	{
		return _tkWindow.IsExiting;
	}
}