using NVorbis;
using OpenTK.Audio.OpenAL;

namespace ocicat.Audio;

/// <summary>Sound data loaded from disk.</summary>
/// <remarks>Only supports ogg files right now.</remarks>
public class Sound
{
	public readonly int Channels;
	public readonly int SampleRate;
	public readonly double TotalTime;
	public readonly int BitsPerSample;
	public List<float> Data = new List<float>();

	public readonly int ALBuffer;

	/// <param name="engine">Audio engine</param>
	/// <param name="path">Path to file</param>
	public Sound(AudioEngine engine, string path)
	{
		Logging.Log(LogLevel.Ocicat, $"Loading sound from {path}");
		
		VorbisReader reader = new VorbisReader(path);

		Channels = reader.Channels;
		SampleRate = reader.SampleRate;
		TotalTime = reader.TotalTime.TotalSeconds;

		Logging.Log(LogLevel.Ocicat, $"Audio info:\n\tChannels: {Channels}\n\tSample rate: {SampleRate}\n\tTotal time: {TotalTime}s");
		
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