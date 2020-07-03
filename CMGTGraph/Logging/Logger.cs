using System;
using System.Diagnostics;
using System.IO;

namespace CMGTGraph.Logging
{
    public static class Logger
    {
        // TODO remove ConditionalAttributes
        // TODO don't reroute the whole console output, only the one from this logger
        
        /// <summary>
        /// The log level of the logger.
        /// </summary>
        public enum LogLevel
        {
            /// <summary>
            /// Print nothing at all.
            /// </summary>
            None,
            /// <summary>
            /// Print only errors. (<see cref="Logger.Error"/>)
            /// </summary>
            Error,
            /// <summary>
            /// Print errors (<see cref="Logger.Error"/>) and warnings (<see cref="Logger.Warn"/>)
            /// </summary>
            Warning,
            /// <summary>
            /// Print errors (<see cref="Logger.Error"/>), warnings (<see cref="Logger.Warn"/>) and logs
            /// (<see cref="Logger.Log"/>).
            /// </summary>
            Verbose,
            /// <summary>
            /// Print everything from every function in this static class.
            /// </summary>
            Spam
        }

        /// <summary>
        /// The default text writer that is saved when the output text writer is changed
        /// (e.g. through <see cref="Writer"/>)
        /// to be able to reset it later.
        /// </summary>
        private static TextWriter _defaultOut;
        
        /// <summary>
        /// When you get it, you get the current text writer of this logger.
        /// <para>
        /// When you set it, the default text writer will be saved in <see cref="_defaultOut"/>
        /// (to be able to reset it) and the out will be set. 
        /// If the provided value is null, nothing happens.
        /// </para>
        /// <para>
        /// In the current implementation, <see cref="Writer"/> is always equal to <see cref="Console.Out"/>,
        /// as it simply sets the out of the console.
        /// </para>
        /// </summary>
        public static TextWriter Writer
        {
            get => Console.Out;
            set
            {
                // TODO don't set the out of the console, instead have a text writer
                // that by default points to the console
                if (value == null) return;
                _defaultOut = Console.Out;
                Console.SetOut(value);
            }
        }

        /// <summary>
        /// Reset the text writer to point to the one it was when changing the text writer through <see cref="Writer"/>. 
        /// If the text writer hasn't been changed, this function does nothing.
        /// </summary>
        public static void ResetWriter()
        {
            if(_defaultOut != null) Console.SetOut(_defaultOut);
        }

        /// <summary>
        /// The current logging level, the logger is operating at.
        /// </summary>
        public static LogLevel LoggingLevel = LogLevel.Error;

        /// <summary>
        /// Spam something to the console in white with the prefix <code>Spam: </code>.
        /// </summary>
        /// <param name="text">The text to spam</param>
        [Conditional("DEBUG")]
        public static void Spam(string text)
        {
            if(LoggingLevel == LogLevel.Spam)
                ColorLog("Spam: " + text);
        }
        
        /// <summary>
        /// Log something to the console in dark blue with the prefix <code>CMGTGraph: </code>.
        /// </summary>
        /// <param name="text">The text to log</param>
        [Conditional("DEBUG")]
        public static void Log(string text)
        {
            if (LoggingLevel == LogLevel.Verbose || LoggingLevel == LogLevel.Spam)
                ColorLog("CMGTGraph: " + text, ConsoleColor.DarkBlue);
        }

        /// <summary>
        /// Log something to the console in yellow with the prefix <code>CMGTGraph [Warning]: </code>.
        /// </summary>
        /// <param name="text">The text to log as a warning</param>
        [Conditional("DEBUG")]
        public static void Warn(string text)
        {
            if(LoggingLevel == LogLevel.Spam || LoggingLevel == LogLevel.Verbose || LoggingLevel == LogLevel.Warning)
                ColorLog("CMGTGraph [Warning]: " + text, ConsoleColor.Yellow);
        }

        /// <summary>
        /// Log something to the console in red with the prefix <code>CMGTGraph [Error]: </code>.
        /// </summary>
        /// <param name="text">The text to log as an error</param>
        [Conditional("DEBUG")]
        public static void Error(string text)
        {
            if(LoggingLevel != LogLevel.None)
                ColorLog("CMGTGraph [Error]: " + text, ConsoleColor.Red);
        }

        /// <summary>
        /// Log something in the specified console color to the current output text writer.
        /// The default color is white.
        /// </summary>
        /// <param name="text">The text to log</param>
        /// <param name="color">The color to log in</param>
        public static void ColorLog(string text, ConsoleColor color = ConsoleColor.White)
        {
            var c = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ForegroundColor = c;
        }
    }
}