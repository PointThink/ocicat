using ocicat;

namespace BasicWindow;

class Game : Application
{
	public Game() : base("Hello world", 800, 600)
	{
	}

	static void Main(string[] args)
	{ 
		new Game().Run();
	}
}