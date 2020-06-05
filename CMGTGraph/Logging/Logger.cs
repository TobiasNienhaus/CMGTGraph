using System;
using System.Diagnostics;

namespace CMGTGraph.Logging
{
    public static class Logger
    {
        public enum LogLevel
        {
            None,
            Error,
            Warning,
            Verbose
        }

        public static LogLevel LoggingLevel = LogLevel.Error;
        
        [Conditional("DEBUG")]
        public static void Log(string text)
        {
            if (LoggingLevel == LogLevel.Verbose)
                ColorLog("CMGTGraph: " + text, ConsoleColor.DarkBlue);
        }

        [Conditional("DEBUG")]
        public static void Warn(string text)
        {
            if(LoggingLevel == LogLevel.Verbose || LoggingLevel == LogLevel.Warning)
                ColorLog("CMGTGraph [Warning]: " + text, ConsoleColor.Yellow);
        }

        [Conditional("DEBUG")]
        public static void Error(string text)
        {
            if(LoggingLevel != LogLevel.None)
                ColorLog("CMGTGraph [Error]: " + text, ConsoleColor.Red);
        }

        public static void ColorLog(string text, ConsoleColor color)
        {
            var c = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ForegroundColor = c;
        }
    }
}