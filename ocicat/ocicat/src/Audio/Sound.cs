using NVorbis;
using OpenTK.Audio.OpenAL;

namespace ocicat.Audio;

public class Sound
{
	public readonly int Channels;
	public readonly int SampleRate;
	public readonly double TotalTime;
	public readonly int BitsPerSample;
	public List<float> Data = new List<float>();

	public readonly int ALBuffer;
	
	public Sound(AudioEngine engine, string path)
	{
		VorbisReader reader = new VorbisReader(path);

		Channels = reader.Channels;
		SampleRate = reader.SampleRate;
		TotalTime = reader.TotalTime.TotalSeconds;
		
		float[] readBuffer = new float[Channels * SampleRate / 5];	
		
		while ((reader.ReadSamples(readBuffer, 0, readBuffer.Length)) > 0)
		{
			foreach (float f in readBuffer)
				Data.Add(f);
		}

		ALBuffer = AL.GenBuffer();

		ALFormat format;

		if (Channels == 1)
			format = ALFormat.MonoFloat32Ext;
		else
			format = ALFormat.StereoFloat32Ext;
		
		AL.BufferData(ALBuffer, format, Data.ToArray(), SampleRate);
		
		engine.CheckAlErrors();
	}

	~Sound()
	{
		AL.DeleteBuffer(ALBuffer);
	}
}