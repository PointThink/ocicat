using ocicat;
using ocicat.Graphics;
using ocicat.Graphics.Rendering;

namespace Animation;

class InGame : GameState
{
	public override void Draw()
	{
		Program.Controller.Draw(Game.Renderer, new Vector2(0, 0), new Vector2(256, 256));
	}
}

class Program
{
	public static AnimationTemplate Template;
	public static AnimationController Controller;
	
	static void Main(string[] args)
	{
		Logging.LogLevel = LogLevel.Ocicat;
		
		Game.Create("Animation", 800, 600);
		
		// created once per animation
		Template = new AnimationTemplate([
			Texture.Create(Game.Renderer, "image.jpg"),
			Texture.Create(Game.Renderer, "image2.png"),
		], 1);

		// create once per object that uses the animation
		Controller = new AnimationController(Template);

		Game.GameState = new InGame();
		Game.Run();
	}
}