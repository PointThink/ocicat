namespace TestGame;

class Program
{
	static void Main(string[] args)
	{
		ocicat.Graphics.Window window = new ocicat.Graphics.Window("Hello", 800, 600);

		while (!window.ShouldClose())
		{
			window.HandleEvents();
			window.SwapBuffers();
		}
	}
}