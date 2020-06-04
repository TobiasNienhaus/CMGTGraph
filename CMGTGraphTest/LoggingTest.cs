using CMGTGraph.Logging;
using NUnit.Framework;

namespace CMGTGraphTest
{
    public class LoggingTest
    {
        [Test]
        public void Test()
        {
            Assert.DoesNotThrow(() => Logger.Log("0"));
            Assert.DoesNotThrow(() => Logger.Warn("0"));
            Assert.DoesNotThrow(() => Logger.Error("0"));
        }
    }
}