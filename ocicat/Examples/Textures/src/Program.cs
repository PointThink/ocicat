using ocicat;
using ocicat.Graphics.Rendering;
using ocicat.Graphics;

namespace Textures;

class Game : Application
{
	public Texture Texture = null!;

	public Game() : base("Textures", 800, 600)
	{
	}

	public override void Initialize()
	{
		Texture = Texture.Create(Renderer, "image.jpg");
	}

	public override void Draw(float deltaTime)
	{
		Renderer.DrawRectTextured(
			new Vector2(0, 0), 
			new Vector2(Texture.GetWidth(), Texture.GetWidth()),
			Texture,
			null,
			0,
			true,
			true
		);
	}

	static void Main(string[] args)
	{
		new Game().Run();
	}
}