using OpenTK.Audio.OpenAL;

namespace ocicat.Audio;

/// <summary>
/// Manages everything audio related.
/// </summary>
public class AudioEngine
{
	private List<AudioHandle> _activeHandles = new List<AudioHandle>();
	
	public AudioEngine()
	{
		ALDevice device = ALC.OpenDevice(null);
		ALContext context = ALC.CreateContext(device, new ALContextAttributes());
		ALC.MakeContextCurrent(context);
		
		// setup listener
		AL.Listener(ALListener3f.Position, 0, 0, 0);
		AL.Listener(ALListener3f.Velocity, 0, 0, 0);
		
		CheckAlErrors();
	}
	
	public AudioHandle PlaySound(Sound sound)
	{
		AudioHandle handle = new AudioHandle(sound);
		handle.Play();
		
		_activeHandles.Add(handle);

		return handle;
	}

	public void CleanFinishedSounds()
	{
		foreach (AudioHandle handle in _activeHandles.ToList())
		{
			if (handle.Finished())
			{
				handle.Destroy();
				_activeHandles.Remove(handle);
			}
		}
	}

	public void CheckAlErrors()
	{
		ALError error = AL.GetError();

		if (error != ALError.NoError)
		{
			Logging.Log(LogLevel.Warning, $"OpenAL error: {Enum.GetName(typeof(ALError), error)}");
		}
	}
}