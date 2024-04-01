using ocicat;
using ocicat.Graphics;
using ocicat.Graphics.Rendering;

namespace Text;

class InGame : GameState
{
	public override void Draw()
	{
		Game.Renderer.DrawText(
			"The quick brown fox jumps over the lazy dog 1234567890",
			Program.Font,
			new Vector2(30, 30),
			Color.CreateFloat(1, 1, 1, 1),
			1f
		);
	}
}

class Program
{
	public static Font Font;
	
	static void Main(string[] args)
	{
		Game.Create("Text", 800, 450);
		
		Font = new Font(Game.Renderer, "OpenSans.ttf", 24);

		Game.GameState = new InGame();
		Game.Run();
	}
}