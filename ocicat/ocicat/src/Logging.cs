namespace ocicat;

public enum LogLevel
{
	Ocicat = 0, // Ocicat debug info
	Developer = 1, // Debug info about game
	Info = 2, // General info (recommended for release)
	Warning = 3, // Warnings
	Error = 4, // Errors
}

struct LogLevelInfo
{
	public LogLevelInfo(ConsoleColor color)
	{
		Color = color;
	}
	
	public readonly ConsoleColor Color;
}

public class Logging
{
	public static LogLevel LogLevel = LogLevel.Info;
	private static StreamWriter? _logFile;
	
	private static readonly Dictionary<LogLevel, LogLevelInfo> LogLevelInfo = new()
	{
		{ LogLevel.Ocicat, new LogLevelInfo(ConsoleColor.Blue) },
		{ LogLevel.Developer, new LogLevelInfo(ConsoleColor.Cyan) },
		{ LogLevel.Info, new LogLevelInfo(ConsoleColor.White) },
		{ LogLevel.Warning, new LogLevelInfo(ConsoleColor.Yellow) },
		{ LogLevel.Error, new LogLevelInfo(ConsoleColor.Red) }
	};

	static Logging()
	{
		_logFile = File.CreateText("ocicat.log");
		_logFile.AutoFlush = true;
		// File.WriteAllText("ocicat.log", string.Empty);
		// _logFile.BaseStream.SetLength(0);
	}
	
	public static void Log(LogLevel level, string text)
	{
		if (level >= LogLevel)
		{
			Console.ForegroundColor = ConsoleColor.Black;
			Console.BackgroundColor = LogLevelInfo[level].Color;
			Console.Write($" {Enum.GetName(typeof(LogLevel), level)} ");
			Console.ResetColor();
			Console.WriteLine($" {text}");

			if (_logFile != null)
			{
				_logFile.Write($"[{Enum.GetName(typeof(LogLevel), level)}] {text}\n");
				// _logFile.Flush();
			}
		}
	}
}