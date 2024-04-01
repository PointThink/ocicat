using OpenTK.Audio.OpenAL;

namespace ocicat.Audio;

public class AudioHandle
{
	private int _source;

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

	public float Pan
	{
		get => AL.GetSource(_source, ALSource3f.Position).X;
		set => AL.Source(_source, ALSource3f.Position, value, 0, 0);
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
		
		AL.Source(_source, ALSourcei.Buffer, sound.ALBuffer);
	}

	~AudioHandle()
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
		return value == (int)ALSourceState.Playing;
	}
}