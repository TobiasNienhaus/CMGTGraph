using System;
using CMGTGraph.Logging;
using NUnit.Framework;

namespace CMGTGraphTest
{
    public class LoggingTest
    {
        [Test]
        public void Test()
        {
            var oldLevel = Logger.LoggingLevel;
            Logger.LoggingLevel = Logger.LogLevel.Spam;
            Assert.IsNotNull(Logger.Writer);
            Assert.DoesNotThrow(() => Logger.Writer = TestContext.Progress);
            Assert.DoesNotThrow(() => Logger.Writer = null);
            Assert.DoesNotThrow(Logger.ResetWriter);
            
            Assert.DoesNotThrow(() => Logger.Spam("Spam"));
            Assert.DoesNotThrow(() => Logger.Log("Log"));
            Assert.DoesNotThrow(() => Logger.Warn("Warn"));
            Assert.DoesNotThrow(() => Logger.Error("Error"));
            Assert.DoesNotThrow(() => Logger.ColorLog("ColorLog", ConsoleColor.Cyan));
            Logger.LoggingLevel = oldLevel;
        }
    }
}