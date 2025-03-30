using ocicat;
using ocicat.Graphics;

namespace Spritesheet;

class Game : Application
{
    private ocicat.Graphics.Spritesheet _spritesheet;
    
    public Game() : base("Spritesheet", 800, 600)
    {
    }

    public override void Initialize()
    {
        _spritesheet = new ocicat.Graphics.Spritesheet(Renderer, "spritesheet.png", "spritesheet.xml");
    }

    public override void Draw(float deltaTime)
    {
        Sprite sprite1 = _spritesheet.GetSprite("sprite1");
        Sprite sprite2 = _spritesheet.GetSprite("sprite2");
        Sprite sprite3 = _spritesheet.GetSprite("sprite3");

        Renderer.DrawSprite(new Vector2(20, 20), sprite1, 3);
        Renderer.DrawSprite(new Vector2(80, 20), sprite2, 3);
        Renderer.DrawSprite(new Vector2(140, 20), sprite3, 3);
    }

    static void Main(string[] args)
    {
        new Game().Run();
    }
}