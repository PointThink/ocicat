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

	private string _title;
	private int _width;
	private int _height;
	private bool _fullscreen;
	private bool _resizable;
	private bool _cursorVisible;
	private bool _cursorLocked;
	
	public override string Title
	{
		get => _title;
		set
		{
			GLFW.SetWindowTitle(_window, value);
			_title = value;
		}
	}

	public override int Width
	{
		get => _width;
		set
		{
			GLFW.SetWindowSize(_window, value, _height);
			_width = value;
			
			OnResize?.Invoke(value, _height);
		}
	}

	public override int Height 
	{
		get => _height;
		set
		{
			GLFW.SetWindowSize(_window, _width, value);
			_height = value;
			
			OnResize?.Invoke(_width, value);
		}
	}

	public override bool Resizable
	{
		get => _resizable;
		set
		{
			GLFW.SetWindowAttrib(_window, WindowAttribute.Resizable, value);
			_resizable = value;
		}
	}
	public override bool Fullscreen
	{
		get => _fullscreen;
		set
		{
			if (value)
				GLFW.SetWindowMonitor(_window, GLFW.GetPrimaryMonitor(), 0, 0, _width, _height, GLFW.DontCare);
			else
				GLFW.SetWindowMonitor(_window, null, 0, 0, _width, _height, GLFW.DontCare);

			_fullscreen = value;
		}
	}

	public override Vector2 MousePosition
	{
		get
		{
			GLFW.GetCursorPos(_window, out double xPos, out double yPos);
			return new Vector2((float)xPos, (float)Height - (float)yPos);
		}
		set => GLFW.SetCursorPos(_window, value.X, value.Y);
	}

	public override bool CursorVisible
	{
		get => _cursorVisible;
		set
		{
			if (!value)
				GLFW.SetInputMode(_window, CursorStateAttribute.Cursor, CursorModeValue.CursorHidden);
			else
				GLFW.SetInputMode(_window, CursorStateAttribute.Cursor, CursorModeValue.CursorHidden);

			_cursorLocked = value;
		}
	}

	public override bool CursorLocked
	{
		get => throw new NotImplementedException();
		set => throw new NotImplementedException();
	}
	
	// yikes
	private OpenTK.Windowing.GraphicsLibraryFramework.Window* _window;

	private GLFWCallbacks.KeyCallback _keyCallback;
	
	public GLFWWindow(string title, int width, int height, bool fullscreen, bool resizable)
	{
		GLFW.Init();
		
		GLFW.WindowHint(WindowHintBool.Resizable, resizable);
		_resizable = resizable;
		
		_window = GLFW.CreateWindow(width, height, title, null, null);
		
		if (fullscreen)
		{
			Monitor* monitor = GLFW.GetPrimaryMonitor();
			GLFW.SetWindowMonitor(_window, monitor, 0, 0, width, height, GLFW.DontCare);
			GLFW.SetWindowSize(_window, width, height);
		}
		
		GLFW.MakeContextCurrent(_window);
		OpenTK.Graphics.OpenGL.GL.LoadBindings(new GLFWBindingsContext());
		OpenTK.Graphics.OpenGL4.GL.LoadBindings(new GLFWBindingsContext());

		_title = title;
		_width = width;
		_height = height;
		_fullscreen = fullscreen;
		_resizable = resizable;
		
		_keyCallback = new GLFWCallbacks.KeyCallback(KeyCallback);
		
		GLFW.SetWindowSizeCallback(_window, ResizeCallback);
		GLFW.SetKeyCallback(_window, _keyCallback);
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
		_width = width;
		_height = height;
		
		OnResize?.Invoke(width, height);
	}
	
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