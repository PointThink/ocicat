using OpenTK.Windowing.GraphicsLibraryFramework;

namespace ocicat.Hardware;

public struct DisplayInfo
{
	public int Width;
	public int Height;
	public int RefreshRate;

	public static unsafe DisplayInfo GetDisplayInfo()
	{
		DisplayInfo displayInfo = new DisplayInfo();

		VideoMode* videoMode = GLFW.GetVideoMode(GLFW.GetPrimaryMonitor());

		displayInfo.Width = videoMode->Width;
		displayInfo.Height = videoMode->Height;
		displayInfo.RefreshRate = videoMode->RefreshRate;
		
		return displayInfo;
	}
}