using ocicat.Audio;
using ocicat.Graphics;
using ocicat.Graphics.Rendering;

namespace ocicat;

public class Application
{
    public Window Window;
    public Renderer Renderer;
    public AudioEngine AudioEngine;
    
    public Application(string title, int width, int height)
    {
        Window = Window.Create(title, width, height);
        Renderer = new Renderer(Window);
        AudioEngine = new AudioEngine();
    }

    public void Run()
    {
        Initialize();

        while (!Window.ShouldClose())
        {
            Window.HandleEvents();
            AudioEngine.CleanFinishedSounds();
            Update(Window.DeltaTime);
            Draw(Window.DeltaTime);
            Window.Present();
        }
            
        Terminate();
    }
    
    public virtual void Initialize() { }
    public virtual void Terminate() { }
    public virtual void Draw(float deltaTime) { }
    public virtual void Update(float deltaTime) { }
}