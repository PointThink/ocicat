using OpenTK.Audio.OpenAL;

namespace ocicat.Audio;

public class AudioEngine
{
	private List<AudioHandle> _activeHandles = new();

	public Vector2 ListenerPosition;
	
	public AudioEngine()
	{
		Logging.Log(LogLevel.Developer, "Initializing OpenAL device");
		
		ALDevice device = ALC.OpenDevice(null);
		ALContext context = ALC.CreateContext(device, new ALContextAttributes());
		ALC.MakeContextCurrent(context);
		
		CheckAlErrors();
		
		// setup listener
		AL.Listener(ALListener3f.Position, 0, 0, 0);
		AL.Listener(ALListener3f.Velocity, 0, 0, 0);
		
		CheckAlErrors();
		
		Logging.Log(LogLevel.Developer, $"Device supports {ALC.GetInteger(device, AlcGetInteger.EfxMaxAuxiliarySends)} effects per source");
	}
	
	public AudioHandle PlaySound(Sound sound)
	{
		AudioHandle handle = new AudioHandle(sound);
		handle.Play();
		
		_activeHandles.Add(handle);

		return handle;
	}
	
	public AudioHandleSpatial PlaySoundSpatial(Sound sound, Vector2 position, float falloff, float volume)
	{
		AudioHandleSpatial handle = new(this, sound);
		
		handle.Falloff = falloff;
		handle.Volume = volume;
		handle.Position = position;
		handle.Play();
		
		_activeHandles.Add(handle);
		return handle;
	}
	
	public AudioHandle CreateHandle(Sound sound)
	{
		AudioHandle handle = new AudioHandle(sound);
		
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