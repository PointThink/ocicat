using ocicat;
using ocicat.Graphics.Rendering;
using ocicat.Graphics;

namespace Textures;

class InGame : GameState
{
	public override void Draw()
	{
		Game.Renderer.DrawRectTextured(
			new Vector2(0, 0), 
			new Vector2(Program.Texture.GetWidth(), Program.Texture.GetWidth()),
			Program.Texture,
			null,
			0,
			true,
			true
		);
	}
}

class Program
{
	public static Texture Texture = null!;
	
	static void Main(string[] args)
	{
		Game.Create("Textures", 800, 600);
		
		Texture = Texture.Create(Game.Renderer, "image.jpg");

		Game.GameState = new InGame();
		Game.Run();
	}
}