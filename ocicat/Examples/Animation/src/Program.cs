using ocicat;
using ocicat.Graphics;
using ocicat.Graphics.Rendering;

namespace Animation;

class Game : Application
{
	public AnimationTemplate Template;
	public AnimationController Controller;

	public Game() : base("Animation", 800, 600)
	{
	}

	public override void Initialize()
	{
		Logging.LogLevel = LogLevel.Ocicat;
		
		// created once per animation
		Template = new AnimationTemplate([
			Texture.Create(Renderer, "image.jpg"),
			Texture.Create(Renderer, "image2.png"),
		], 1);

		// create once per object that uses the animation
		Controller = new AnimationController(Template);
	}

	public override void Draw(float deltaTime)
	{
		Controller.Draw(Renderer, new Vector2(0, 0), new Vector2(256, 256));
	}

	static void Main(string[] args)
	{
		new Game().Run();
	}
}