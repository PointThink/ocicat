using ocicat.Graphics;

namespace ocicat.Input;

public abstract class Binding
{
	public abstract bool IsDown(Window window);
	public abstract bool IsPressed(Window window);
}

public class KeyboardBind : Binding
{
	private Key _key;
	
	public KeyboardBind(Key key)
	{
		_key = key;
	}
	
	public override bool IsDown(Window window)
	{
		return window.IsKeyDown(_key);
	}

	public override bool IsPressed(Window window)
	{
		return window.IsKeyPressed(_key);
	}
}

public class MouseBind : Binding
{
	private int _button;
	
	public MouseBind(int button)
	{
		_button = button;
	}
    
	public override bool IsDown(Window window)
	{
		return window.IsMouseButtonDown(_button);
	}

	public override bool IsPressed(Window window)
	{
		return window.IsMouseButtonPressed(_button);
	}
}

public class Bindings
{
	private Dictionary<string, Binding> _bindings = new();
	private Window _window;
	
	public Bindings(Window window)
	{
		_window = window;
	}
	
	public void AddBinding(string name, Binding binding)
	{
		_bindings.Add(name, binding);
	}

	public void Clear()
	{
		_bindings.Clear();
	}

	public bool IsDown(string name)
	{
		if (!_bindings.ContainsKey(name))
			return false;
			
		return _bindings[name].IsDown(_window);
	}
	
	public bool IsPressed(string name)
	{
		if (!_bindings.ContainsKey(name))
			return false;
			
		return _bindings[name].IsPressed(_window);
	}
}