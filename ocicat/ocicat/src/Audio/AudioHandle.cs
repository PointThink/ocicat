using System.Numerics;
using OpenTK.Audio.OpenAL;

namespace ocicat.Audio;

public class AudioHandle
{
	private int _source;
	public bool ManualCleanup = false;

	public float Volume
	{
		get => AL.GetSource(_source, ALSourcef.Gain);
		set => AL.Source(_source, ALSourcef.Gain, value);
	}
	
	public float Pitch
	{
		get => AL.GetSource(_source, ALSourcef.Pitch);
		set => AL.Source(_source, ALSourcef.Pitch, value);
	}

	/// <summary>
	/// Speaker volume for stereo. -1 is left, 1 is right.
	/// </summary>
	public float Pan
	{
		get => AL.GetSource(_source, ALSource3f.Position).X;
		set => AL.Source(_source, ALSource3f.Position, value, 0f, (float) -Math.Sqrt(1f - value * value));
	}
	
	public AudioHandle(Sound sound)
	{
		_source = AL.GenSource();
		AL.Source(_source, ALSourcef.Pitch, 1);
		AL.Source(_source, ALSourcef.Gain, 1);
		AL.Source(_source, ALSource3f.Position, 0, 0, 0);
		AL.Source(_source, ALSource3f.Velocity, 0, 0, 0);
		AL.Source(_source, ALSourceb.Looping, false);
	
		AL.Source(_source, ALSourcef.MaxGain, 10);
		AL.Source(_source, ALSourcef.MinGain, 0);
		
		AL.Source(_source, ALSourcef.RolloffFactor, 0);
		AL.Source(_source, ALSourceb.SourceRelative, true);
		
		AL.Source(_source, ALSourcei.Buffer, sound.ALBuffer);
	}

	public void Destroy()
	{
		AL.DeleteSource(_source);
	}

	public void Play()
	{
		AL.SourcePlay(_source);
	}

	public void Stop()
	{
		AL.SourceStop(_source);
	}

	public void Pause()
	{
		AL.SourcePause(_source);
	}

	public void Rewind()
	{
		AL.SourceRewind(_source);
	}

	public bool Finished()
	{
		AL.GetSource(_source, ALGetSourcei.SourceState, out int value);
		return value != (int)ALSourceState.Playing && !ManualCleanup;
	}
}

public class AudioHandleSpatial : AudioHandle
{
	private AudioEngine _engine;
	
	public AudioHandleSpatial(AudioEngine engine, Sound sound) : base(sound)
	{
		_engine = engine;
	}
	
	private Vector2 _position;

	public Vector2 Position
	{
		get => _position;
		set
		{
			_position = value;
			float desiredVolume = (Falloff - Vector2.GetDistance(value, _engine.ListenerPosition)) / Falloff;
			desiredVolume = desiredVolume < 0 ? 0 : desiredVolume;
			Volume = desiredVolume;

			float panDistance = Vector2.GetDistance(value, _engine.ListenerPosition);
			
			if (_engine.ListenerPosition.X - value.X > 0)
				panDistance *= -1;
			
			Pan = ( panDistance / Falloff ) / 2;
		}
	}
	
	public float Falloff;
}