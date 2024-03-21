namespace ocicat.Graphics.Rendering;

public enum RenderingApi
{
	OpenGl
}

public class Renderer
{
	public RenderingApi RenderingApi { get; private set; } 
	public RenderCommands RenderCommands { get; private set; }
    
	public Renderer(RenderingApi renderingApi)
	{
		RenderingApi = renderingApi;
		RenderCommands = RenderCommands.Create(this);
		RenderCommands.Init();
	}
}