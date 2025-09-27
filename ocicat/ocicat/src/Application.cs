using ocicat.Audio;
using ocicat.Graphics;
using ocicat.Graphics.Rendering;

namespace ocicat;

public class Application
{
    public Window Window;
    public Renderer Renderer;
    public AudioEngine AudioEngine;
    public Camera Camera;

    public Application(string title, int width, int height)
    {
        Window = Window.Create(title, width, height);
        Renderer = new Renderer(Window);
        AudioEngine = new AudioEngine();
        Camera = new OrthographicCamera(width, height);
    }

    public void Run()
    {
        Initialize();

        while (!Window.ShouldClose())
        {
            Window.HandleEvents();
            AudioEngine.CleanFinishedSounds();
            Update(Window.DeltaTime);

            Renderer.BeginScene(Camera);
            Draw(Window.DeltaTime);
            Renderer.EndScene();
            Window.Present();
        }
            
        Terminate();
    }
    
    public virtual void Initialize() { }
    public virtual void Terminate() { }
    public virtual void Draw(float deltaTime) { }
    public virtual void Update(float deltaTime) { }
}