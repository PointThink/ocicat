using ocicat.Graphics;
using ocicat.Graphics.Rendering;
using Vector2 = ocicat.Vector2;

namespace TestGame;

static class Game
{
	private static Window? _window;
	private static Renderer? _renderer;
	
	static void Main()
	{
		_window = Window.Create("Hello", 1280, 720);
		
		_renderer = new Renderer(_window);
		_renderer.RenderCommands.SetClearColor(0.2f, 0.2f, 0.2f, 1f);

		AnimationTemplate animationTemplate = new AnimationTemplate([
			Texture.Create(_renderer, "image.jpg"),
			Texture.Create(_renderer, "image2.png")
		], 1);

		AnimationController animationController = new AnimationController(animationTemplate);

		while (!_window.ShouldClose())
		{
			_window.HandleEvents();
			_renderer.BeginDrawing();
			
			_renderer.RenderCommands.ClearScreen();
			animationController.Draw(_renderer, _window.GetMouseMotion(), new Vector2(64, 64));
			// _renderer.DrawRect(new Vector2(64, 64), new Vector2(64, 64), Color.CreateFloat(1, 1, 1, 1));
			
			_renderer.DrawRoundedRect(new Vector2(64, 64), new Vector2(256, 128), 64);
			
			_renderer.EndDrawing();	
			_window.Present();
		}
	}
}