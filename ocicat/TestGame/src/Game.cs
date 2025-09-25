using ocicat;
using ocicat.Graphics;
using Vector2 = ocicat.Vector2;

namespace TestGame;

public class Game : Application
{
    private TexturedBatch _texturedQuadBatch;
    private RNG _rng = new RNG();

    public Game() : base("Test game", 800, 600)
    {
        _texturedQuadBatch = new TexturedBatch(Renderer, 100);
    }

    public override void Initialize()
    {
        Console.WriteLine("Initializing Game");
    }

    public override void Draw(float deltaTime)
    {
        Renderer.ClearScreen(Color.Black);

        for (int i = 0; i < 100; i++)
        {
            BatchedVertex[] batchedVertices = _texturedQuadBatch.CreateQuad(
                new Vector2(_rng.GenerateFloat(0, 800), _rng.GenerateFloat(0, 600)),
                new Vector2(40, 40),
                Color.CreateFloat(_rng.GenerateFloat(0, 1), _rng.GenerateFloat(0, 1), _rng.GenerateFloat(0, 1), 1)
            );

            _texturedQuadBatch.AddQuad(batchedVertices);
        }

        _texturedQuadBatch.Render(Renderer, Renderer.Camera);
    }

    public override void Update(float deltaTime)
    {
        // Console.WriteLine($"{1 / deltaTime} FPS");
    }

    static void Main(string[] args)
    {
        new Game().Run();
    }
}