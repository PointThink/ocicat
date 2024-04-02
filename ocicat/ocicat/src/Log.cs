namespace ocicat;

public enum LogLevel
{
	Ocicat = 0, // Ocicat debug info
	Developer = 1, // Debug info about game
	Info = 2, // General info (recommended for release)
	Warning = 3, // Warnings
	Error = 4, // Errors
}

class LogLevelInfo
{
	public LogLevelInfo(string label, ConsoleColor color)
	{
		Label = label;
		Color = color;
	}
		
	public string Label;
	public ConsoleColor Color;
}

public class Logging
{
	public static LogLevel LogLevel = LogLevel.Info;

	private static Dictionary<LogLevel, LogLevelInfo> _logLevelInfo = new Dictionary<LogLevel, LogLevelInfo>()
	{
		{ LogLevel.Ocicat, new LogLevelInfo("Ocicat", ConsoleColor.Blue) },
		{ LogLevel.Developer, new LogLevelInfo("Developer", ConsoleColor.Cyan) },
		{ LogLevel.Info, new LogLevelInfo("Info", ConsoleColor.White) },
		{ LogLevel.Warning, new LogLevelInfo("Warning", ConsoleColor.Yellow) },
		{ LogLevel.Error, new LogLevelInfo("Error", ConsoleColor.Red) }
	};
	
	public static void Log(LogLevel level, string text)
	{
		if (level >= LogLevel)
		{
			Console.ForegroundColor = ConsoleColor.Black;
			Console.BackgroundColor = _logLevelInfo[level].Color;
			Console.Write($" {_logLevelInfo[level].Label} ");
			Console.ResetColor();
			Console.WriteLine($" {text}");
		}
	}
}