namespace ocicat;

public class RNG
{
	private Random _random = new ();

	public int GenerateInt(int min, int max)
	{
		return _random.Next(min, max);
	}

	public float GenerateFloat(float min, float max)
	{
		return _random.NextSingle() * (max - min + 1) + min;
	}

	public double GenerateDouble(double min, double max)
	{
		return _random.NextDouble() * (max - min + 1) + min;
	}
}