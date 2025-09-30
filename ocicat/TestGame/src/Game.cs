using ocicat;
using ocicat.Graphics;
using ocicat.Graphics.Rendering;
using OpenTK.Windowing.GraphicsLibraryFramework;
using Vector2 = ocicat.Vector2;

namespace TestGame;

public class Game : Application
{
    private RNG _rng = new RNG();

    private Texture?[] _textures;

    private float _rotation = 0;
    private Font _font;
    private float _averageFps = 0;

    private bool _batch = false;

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

        for (int i = 0; i < 1; i++)
        {

            if (_batch)
            {
                /*
                Renderer.DrawRectTextured(
                    new Vector2(_rng.GenerateFloat(0, 800), _rng.GenerateFloat(0, 600)),
                    new Vector2(40, 40),
                    _textures[_rng.GenerateInt(0, 2)],
                    Color.White,
                    0
                );
                */
            }
            else
            {
                Renderer.DrawRectTexturedUnbatched(
                    new Vector2(_rng.GenerateFloat(0, 800), _rng.GenerateFloat(0, 600)),
                    new Vector2(40, 40),
                    _textures[_rng.GenerateInt(0, 2)],
                    Color.White,
                    0
                ); 
            } 
        }

        Renderer.DrawText($"{(int) _averageFps} FPS", _font, new Vector2(10, 10), Color.White);

        _rotation += deltaTime * 30;
    }

    public override void Update(float deltaTime)
    {
        if (Window.IsKeyPressed(ocicat.Input.Key.Space))
            _batch = !_batch;

        if (_averageFps == 0)
                _averageFps = deltaTime;
            else
                _averageFps = (_averageFps + 1 / deltaTime) / 2;
    }

    static void Main(string[] args)
    {
        new Game().Run();
    }
}