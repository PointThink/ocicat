using ocicat.Graphics.Rendering;
using System.Numerics;

namespace ocicat.Graphics;

public class AnimationTemplate
{
	public AnimationTemplate(Texture[] frames, float fps)
	{
		Frames = frames;
		BaseFps = fps;
	}

	public readonly Texture[] Frames;
	public readonly float BaseFps;
}

public class AnimationController
{
	public readonly AnimationTemplate Template;

	public AnimationController(AnimationTemplate template)
	{
		Template = template;
	}

	public void Reset()
	{
		_lastFrameTime = DateTime.Now.TimeOfDay;
		_currentFrame = 0;
	}

	public void Draw(Renderer renderer, Vector2 position, Vector2 size, Color tint = null, float rotation = 0)
	{	
		float actualFps = Template.BaseFps * SpeedMultiplier;

		if (_lastFrameTime + TimeSpan.FromMilliseconds(1000 / actualFps) < DateTime.Now.TimeOfDay)
		{
			_currentFrame++;
			_lastFrameTime = DateTime.Now.TimeOfDay;
		}

		if (_currentFrame >= Template.Frames.Length)
			_currentFrame = 0;
		
		renderer.DrawRectTextured(position, size, Template.Frames[_currentFrame], tint, rotation);
	}

	public float SpeedMultiplier = 1;

	private int _currentFrame = 0;
	private TimeSpan _lastFrameTime = TimeSpan.Zero;
}