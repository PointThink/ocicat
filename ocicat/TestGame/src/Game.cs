using System.Numerics;
using ocicat;
using ocicat.Audio;
using ocicat.Graphics;
using ocicat.Input;
using Vector2 = ocicat.Vector2;

namespace TestGame;

public class Game : Application
{
    public Game() : base("Test game", 800, 600)
    {
    }

    public override void Initialize()
    {
        Console.WriteLine("Initializing Game");
    }

    public override void Draw(float deltaTime)
    {
        Renderer.ClearScreen(Color.Blue);
        Renderer.DrawRect(new Vector2(0, 0), new Vector2(40, 40), Color.Red);
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