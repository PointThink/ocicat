using ocicat;
using ocicat.Graphics;
using ocicat.Graphics.Rendering;
using Vector2 = ocicat.Vector2;

namespace TestGame;

public class Game : Application
{
    private RNG _rng = new RNG();

    private Texture?[] _textures;

    private float _rotation = 0;
    private Font _font;

    public Game() : base("Test game", 800, 600)
    {
        _textures = [
            Texture.Create(Renderer, "image.jpg"),
            Texture.Create(Renderer, "image2.png")
        ];

        _font = new Font(Renderer, "fonts/Roboto-Regular.ttf", 32);
    }

    public override void Initialize()
    {
        Console.WriteLine("Initializing Game");
    }

    public override void Draw(float deltaTime)
    {
        Renderer.ClearScreen(Color.Black);

        for (int i = 0; i < 10000; i++)
        {
            Renderer.DrawRectTextured(
                new Vector2(_rng.GenerateFloat(0, 800), _rng.GenerateFloat(0, 600)),
                new Vector2(40, 40),
                _textures[_rng.GenerateInt(0, 2)],
                Color.White,
                0
            );
        }

        Renderer.DrawText($"{1 / deltaTime} FPS", _font, new Vector2(10, 10), Color.White);

        _rotation += deltaTime * 30;
    }

    public override void Update(float deltaTime)
    {
        Console.WriteLine($"{1 / deltaTime} FPS");
    }

    static void Main(string[] args)
    {
        new Game().Run();
    }
}