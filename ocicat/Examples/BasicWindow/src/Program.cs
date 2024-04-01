using ocicat;

namespace BasicWindow;

class Program
{
	static void Main(string[] args)
	{
		Game.Create("Hello world", 800, 600);
		Game.Run();
	}
}