using System.Numerics;
using OpenTK.Audio.OpenAL;

namespace ocicat.Audio;

public class AudioSource
{
	public readonly int AlSource;

	public Vector3 Position
	{
		get
		{
			AL.GetSource(AlSource, ALSource3f.Position, out OpenTK.Mathematics.Vector3 position);
			return new Vector3(position.X, position.Y, position.Z);
		}
		set => AL.Source(AlSource, ALSource3f.Position, value.X, value.Y, value.Z);
	}

	public Vector3 Velocity
    {
    	get
    	{
    		AL.GetSource(AlSource, ALSource3f.Velocity, out OpenTK.Mathematics.Vector3 velocity);
    		return new Vector3(velocity.X, velocity.Y, velocity.Z);
    	}
    	set => AL.Source(AlSource, ALSource3f.Velocity, value.X, value.Y, value.Z);
    }
	
	public AudioSource(Vector3 position)
	{
		AlSource = AL.GenSource();

		Position = position;
		Velocity = Vector3.Zero;
	}
}