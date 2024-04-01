using ocicat;
using ocicat.Graphics;
using ocicat.Graphics.Rendering;

namespace Shapes;

class InGame : GameState
{
	float fps;
	double nextFpsUpdate;
	Font font;
	
	public override void OnEnter()
	{
		font = new Font(Game.Renderer, "OpenSans.ttf", 24);
		nextFpsUpdate = Game.Window.Time;
	}

	public override void Draw()
	{
		Game.Renderer.DrawRect(new Vector2(200, 200), new Vector2(200, 150), Color.CreateFloat(0.2f, 0.2f, 1, 1));
		Game.Renderer.DrawCircle(new Vector2(600, 100), 64, 32, Color.CreateFloat(1f, 1, 0.2f, 1));
		Game.Renderer.DrawTriangle(new Vector2(50, 50), new Vector2(100, 150), new Vector2(150, 50), Color.CreateFloat(1, 0.2f, 0.2f, 1));
		Game.Renderer.DrawRoundedRect(new Vector2(600, 300), new Vector2(100, 150), 30, Color.CreateFloat(1, 0.2f, 1, 1));

		if (Game.Window.Time > nextFpsUpdate)
		{
			nextFpsUpdate = Game.Window.Time + 1;
			fps = 1f / Game.Window.DeltaTime;
		}
			
		Game.Renderer.DrawText($"FPS: {fps}", font, new Vector2(20, 20));

	}
}

class Program
{
	static void Main(string[] args)
	{
		Game.Create("Shapes", 800, 600);

		Game.GameState = new InGame();
		Game.Run();
	}
}