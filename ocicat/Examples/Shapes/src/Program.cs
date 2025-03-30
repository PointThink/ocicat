using ocicat;
using ocicat.Graphics;
using ocicat.Graphics.Rendering;

namespace Shapes;

class Game : Application
{
	float fps;
	double nextFpsUpdate;
	Font font;
	
	public Game() : base("Shapes", 800, 600) {}

	public override void Initialize()
	{
		font = new Font(Renderer, "OpenSans.ttf", 24);
		nextFpsUpdate = Window.Time;
	}

	public override void Draw(float deltaTime)
	{
		Renderer.ClearScreen(Color.CreateFloat(0.2f, 0.2f, 0.2f, 1));
		
		Renderer.DrawRect(new Vector2(200, 200), new Vector2(200, 150), Color.CreateFloat(0.2f, 0.2f, 1, 1));
		Renderer.DrawCircle(new Vector2(600, 100), 64, 32, Color.CreateFloat(1f, 1, 0.2f, 1));
		Renderer.DrawTriangle(new Vector2(50, 50), new Vector2(100, 150), new Vector2(150, 50), Color.CreateFloat(1, 0.2f, 0.2f, 1));
		Renderer.DrawRoundedRect(new Vector2(600, 300), new Vector2(100, 150), 30, Color.CreateFloat(1, 0.2f, 1, 1));

		if (Window.Time > nextFpsUpdate)
		{
			nextFpsUpdate = Window.Time + 1;
			fps = 1f / Window.DeltaTime;
		}
			
		Renderer.DrawText($"FPS: {fps}", font, new Vector2(20, 20));
	}
	
	static void Main(string[] args)
	{
		new Game().Run();
	}
}