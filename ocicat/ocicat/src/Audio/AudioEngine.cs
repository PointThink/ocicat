using System.Diagnostics.Tracing;
using System.Numerics;
using OpenTK.Audio.OpenAL;

namespace ocicat.Audio;

/// <summary>
/// Manages everything audio related.
/// </summary>
public class AudioEngine
{
	private List<AudioHandle> _activeHandles = new List<AudioHandle>();

	public Vector3 ListenerPosition
	{
		get
		{
			AL.GetListener(ALListener3f.Position, out OpenTK.Mathematics.Vector3 position);
			return new Vector3(position.X, position.Y, position.Z);
		}
		set => AL.Listener(ALListener3f.Position, value.X, value.Y, value.Z);
	}

	public Vector3 ListenerVelocity
    {
    	get
    	{
    		AL.GetListener(ALListener3f.Velocity, out OpenTK.Mathematics.Vector3 velocity);
    		return new Vector3(velocity.X, velocity.Y, velocity.Z);
    	}
    	set => AL.Listener(ALListener3f.Velocity, value.X, value.Y, value.Z);
    }	
	
	public AudioEngine()
	{
		Logging.Log(LogLevel.Ocicat, "Initializing OpenAL device");
		
		ALDevice device = ALC.OpenDevice(null);
		ALContext context = ALC.CreateContext(device, new ALContextAttributes());
		ALC.MakeContextCurrent(context);
		
		CheckAlErrors();
		
		// setup listener
		AL.Listener(ALListener3f.Position, 0, 0, 0);
		AL.Listener(ALListener3f.Velocity, 0, 0, 0);
		
		CheckAlErrors();
		
		Logging.Log(LogLevel.Ocicat, $"Device supports {ALC.GetInteger(device, AlcGetInteger.EfxMaxAuxiliarySends)} effects per source");
	}
	
	public AudioHandle PlaySound(Sound sound)
	{
		AudioHandle handle = new AudioHandle(sound);
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

	public void PlaySoundFromSource(Sound sound, AudioSource source)
	{
		AL.Source(source.AlSource, ALSourcei.Buffer, sound.ALBuffer);
		AL.SourcePlay(source.AlSource);
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