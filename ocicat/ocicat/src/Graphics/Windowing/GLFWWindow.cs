using ocicat.Input;
using OpenTK.Graphics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using Monitor = OpenTK.Windowing.GraphicsLibraryFramework.Monitor;

namespace ocicat.Graphics;

public unsafe class GLFWWindow : Window
{
	private DateTime _frameBeginTime;

	private bool[] _keyDownState = new bool[(int) Key.KeysCount];
	private bool[] _keyPressedState = new bool[(int) Key.KeysCount];
	private bool[] _mouseDownState = new bool[255];
	private bool[] _mousePressedState = new bool[255];

	private bool _resizable;
	
	// yikes
	private OpenTK.Windowing.GraphicsLibraryFramework.Window* _window;
	
	public GLFWWindow(string title, int width, int height, bool fullscreen, bool resizable)
	{
		GLFW.Init();
		_window = GLFW.CreateWindow(width, height, title, null, null);
		
		if (!resizable)
			GLFW.SetWindowSizeLimits(_window, width, height, width, height);
		
		if (fullscreen)
		{
			Monitor* monitor = GLFW.GetPrimaryMonitor();
			GLFW.SetWindowMonitor(_window, monitor, 0, 0, width, height, GLFW.DontCare);
			GLFW.SetWindowSize(_window, width, height);
		}
		
		GLFW.MakeContextCurrent(_window);
		OpenTK.Graphics.OpenGL.GL.LoadBindings(new GLFWBindingsContext());
		OpenTK.Graphics.OpenGL4.GL.LoadBindings(new GLFWBindingsContext());
		
		Width = width;
		Height = height;
		_resizable = resizable;
        
		GLFW.SetWindowSizeCallback(_window, ResizeCallback);
		GLFW.SetKeyCallback(_window, KeyCallback);
		GLFW.SetMouseButtonCallback(_window, MouseButtonCallback);
	}

	private void MouseButtonCallback(OpenTK.Windowing.GraphicsLibraryFramework.Window* window, MouseButton button, InputAction action, KeyModifiers mods)
	{
		if (action == InputAction.Press)
		{
			_mouseDownState[(int) button] = true;
			_mousePressedState[(int) button] = true;
		}
		else
			_mouseDownState[(int) button] = false;
	}

	private void KeyCallback(OpenTK.Windowing.GraphicsLibraryFramework.Window* window, Keys key, int scancode, InputAction action, KeyModifiers mods)
	{
		if (action == InputAction.Press)
		{
			_keyDownState[(int) key] = true;
			_keyPressedState[(int) key] = true;
		}
		if (action == InputAction.Release)
			_keyDownState[(int) key] = false;
	}

	private void ResizeCallback(OpenTK.Windowing.GraphicsLibraryFramework.Window* window, int width, int height)
	{
		Width = width;
		Height = height;
		
		OnResize?.Invoke(width, height);
	}

	public override bool CursorVisible { get; set; }
	public override bool CursorLocked { get; set; }
	
	public override void HandleEvents()
	{
		for (int i = 0; i < _keyPressedState.Length; i++)
			_keyPressedState[i] = false;
		
		for (int i = 0; i < _mousePressedState.Length; i++)
			_mousePressedState[i] = false;
		
		_frameBeginTime = DateTime.Now;
		
		GLFW.WaitEvents();
	}

	public override void Present()
	{
		GLFW.SwapBuffers(_window);
		DeltaTime = ( (float) (DateTime.Now - _frameBeginTime).TotalMilliseconds ) / 1000;
		Time += DeltaTime;
	}

	public override bool ShouldClose()
	{
		return GLFW.WindowShouldClose(_window);
	}

	public override bool IsKeyDown(Key key)
	{
		return _keyDownState[(int) key];
	}

	public override bool IsKeyPressed(Key key)
	{
		return _keyPressedState[(int) key];
	}

	public override bool IsMouseButtonDown(int button)
	{
		return _mouseDownState[button];
	}

	public override bool IsMouseButtonPressed(int button)
	{
		return _mousePressedState[button];
	}

	public override Vector2 GetMousePosition()
	{
		GLFW.GetCursorPos(_window, out double xPos, out double yPos);
		return new Vector2((float)xPos, (float)Height - (float)yPos);
	}
}