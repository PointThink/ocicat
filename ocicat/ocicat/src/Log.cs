namespace ocicat;

public enum LogLevel
{
	Developer = 0,
	Info = 1,
	Warning = 2,
	Error = 3,
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
		{ LogLevel.Developer, new LogLevelInfo("Developer", ConsoleColor.Blue) },
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