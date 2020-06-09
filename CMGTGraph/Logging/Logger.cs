using System;
using System.Diagnostics;
using System.IO;

namespace CMGTGraph.Logging
{
    public static class Logger
    {
        public enum LogLevel
        {
            None,
            Error,
            Warning,
            Verbose,
            Spam
        }

        private static TextWriter _defaultOut;
        
        public static TextWriter Writer
        {
            get => Console.Out;
            set
            {
                if (value == null) return;
                _defaultOut = Console.Out;
                Console.SetOut(value);
            }
        }

        public static void ResetWriter()
        {
            if(_defaultOut != null) Console.SetOut(_defaultOut);
        }

        public static LogLevel LoggingLevel = LogLevel.Error;

        [Conditional("DEBUG")]
        public static void Spam(string text)
        {
            if(LoggingLevel == LogLevel.Spam)
                ColorLog("Spam: " + text, ConsoleColor.White);
        }
        
        [Conditional("DEBUG")]
        public static void Log(string text)
        {
            if (LoggingLevel == LogLevel.Verbose || LoggingLevel == LogLevel.Spam)
                ColorLog("CMGTGraph: " + text, ConsoleColor.DarkBlue);
        }

        [Conditional("DEBUG")]
        public static void Warn(string text)
        {
            if(LoggingLevel == LogLevel.Spam || LoggingLevel == LogLevel.Verbose || LoggingLevel == LogLevel.Warning)
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