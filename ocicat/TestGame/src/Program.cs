using ocicat;
using ocicat.Graphics;

namespace TestGame;

class Program
{
	static void Main(string[] args)
	{
		Window window = new Window("Hello", 800, 600);

		Logging.LogLevel = LogLevel.Developer;
		
		Logging.Log(LogLevel.Developer, "Hello");
		Logging.Log(LogLevel.Info, "Hello");
		Logging.Log(LogLevel.Warning, "Hello");
		Logging.Log(LogLevel.Error, "Hello");
		
		while (!window.ShouldClose())
		{
			window.HandleEvents();
			window.SwapBuffers();
		}
	}
}