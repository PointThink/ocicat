using OpenTK.Mathematics;

namespace ocicat.Graphics.Rendering;

public class RenderTarget
{
    public readonly int Width;
    public readonly int Height;

    public Framebuffer Framebuffer { get; private set; }
    public OrthographicCamera Camera;

    public Texture Texture => Framebuffer.GetTextureAttachment();

    public RenderTarget(Renderer renderer, int width, int height)
    {
        Width = width;
        Height = height;

        Framebuffer = Framebuffer.Create(renderer, width, height);
        Camera = new OrthographicCamera(width, height);
    }
}
