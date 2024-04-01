using System.Numerics;
using OpenTK.Audio.OpenAL;

namespace ocicat.Audio;

/// <summary>
/// Controls sound played by the audio engine
/// </summary>
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

	/// <summary>
	/// Speaker volume for stereo. -1 is left, 1 is right.
	/// </summary>
	public float Pan
	{
		get => AL.GetSource(_source, ALSource3f.Position).X;
		set => AL.Source(_source, ALSource3f.Position, value, 1 - value, 0);
	}

	
	/// <summary>
	/// Position for spatial sound.
	/// </summary>
	public Vector3 Position
	{
		get
		{
			OpenTK.Mathematics.Vector3 tkVec = AL.GetSource(_source, ALSource3f.Position);
			return new Vector3(tkVec.X, tkVec.Y, tkVec.Z);
		}
		set => AL.Source(_source, ALSource3f.Position, value.X, value.Y, value.Z);
	}
	
	
	/// <summary>
	/// Velocity of the sound source. I honestly don't know what this does.
	/// The audio system uses OpenAL, so I guess you can look it up in the OpenAL docs.
	/// </summary>
	public Vector3 Velocity
	{
		get
		{
			OpenTK.Mathematics.Vector3 tkVec = AL.GetSource(_source, ALSource3f.Velocity);
			return new Vector3(tkVec.X, tkVec.Y, tkVec.Z);
		}
		set => AL.Source(_source, ALSource3f.Velocity, value.X, value.Y, value.Z);
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
		return value != (int)ALSourceState.Playing;
	}
}