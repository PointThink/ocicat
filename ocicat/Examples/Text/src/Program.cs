using ocicat;
using ocicat.Graphics;
using ocicat.Graphics.Rendering;

namespace Text;

class Game : Application
{
	public Font Font;

	public Game() : base("Text", 800, 450)
	{
	}

	public override void Initialize()
	{
		Font = new Font(Renderer, "OpenSans.ttf", 24);
	}

	public override void Draw(float deltaTime)
	{
		Renderer.ClearScreen(Color.Blue);
		Renderer.DrawText(
			"The quick brown fox jumps over the lazy dog 1234567890",
			Font,
			new Vector2(30, 30),
			Color.CreateFloat(1, 1, 1, 1),
			1f
		);
	}

	static void Main(string[] args)
	{
		new Game().Run();
	}
}