using System;
using System.Diagnostics;

namespace CMGTGraph.Logging
{
    public static class Logger
    {
        [Conditional("DEBUG")]
        public static void Log(string text)
        {
            Console.WriteLine(text);
        }

        [Conditional("DEBUG")]
        public static void Warn(string text)
        {
            var c = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(text);
            Console.ForegroundColor = c;
        }

        [Conditional("DEBUG")]
        public static void Error(string text)
        {

            var c = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(text);
            Console.ForegroundColor = c;
        }
    }
}