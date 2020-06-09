using System;
using System.Collections.Generic;
using CMGTGraph;
using CMGTGraph.Algorithms;
using CMGTGraph.Logging;
using CMGTGraph.Types;
using NUnit.Framework;

namespace CMGTGraphTest.AlgorithmsTests.Recursive
{
    public class RecursivePathTest
    {
        private Graph<Point> _testGraph;
        private Logger.LogLevel _oldLevel;
        
        [SetUp]
        public void SetUp()
        {
            _oldLevel = Logger.LoggingLevel;
            Logger.LoggingLevel = Logger.LogLevel.Verbose;
            
            Logger.Log("Started testing");
            
            _testGraph = new Graph<Point>(Point.Calculator);
            _testGraph.Add(new Point(0, 1));
            _testGraph.AddConnection(new Point(0, 1), new Point(1, 2));
            _testGraph.AddConnection(new Point(2, 3), new Point(3, 4));
            _testGraph.AddConnection(new Point(3, 4), new Point(1, 2));
            // separate net
            _testGraph.AddConnection(new Point(800, 100), new Point(900, 100));
            _testGraph.AddConnection(new Point(900, 100), new Point(850, 1000));
            _testGraph.AddConnection(new Point(800, 100), new Point(850, 1000));
            _testGraph.AddConnection(new Point(850, 1000), new Point(950, 800));
            _testGraph.AddConnection(new Point(950, 800), new Point(20, 80));
            _testGraph.AddConnection(new Point(950, 800), new Point(800, 100));
        }

        [TearDown]
        public void TearDown()
        {
            Logger.Log("Finished Testing");
            Logger.LoggingLevel = _oldLevel;
        }

        [Test, Timeout(5000)]
        public void ValidPathTest()
        {
            var path = _testGraph.RecursiveSolve(new Point(0, 1), new Point(3, 4));
            Assert.IsNotEmpty(path);
            PrintPath(path);

            path = _testGraph.RecursiveSolve(new Point(800, 100), new Point(20, 80));
            Assert.IsNotEmpty(path);
            PrintPath(path);
        }

        [Test, Timeout(5000)]
        public void InvalidPathTest()
        {
            var path = _testGraph.RecursiveSolve(new Point(0, 1), new Point(950, 800));
            Assert.IsEmpty(path);
            PrintPath(path);
        }

        [Test]
        public void InvalidParametersTest()
        {
            Assert.Catch<Graph<Point>.NodeNotFoundException>(() =>
                _testGraph.RecursiveSolve(new Point(-1, -1), new Point(950, 800)));
            Assert.Catch<Graph<Point>.NodeNotFoundException>(() =>
                _testGraph.RecursiveSolve(new Point(950, 800), new Point(-1, -1)));
            Assert.Catch<Graph<Point>.NodeNotFoundException>(() =>
                _testGraph.RecursiveSolve(new Point(-2, -2), new Point(-1, -1)));
        }

        private static void PrintPath<T>(List<T> path) where T : IEquatable<T>
        {
            Logger.Log("Path");
            if(path.Count == 0)
                Logger.Log("Path is empty");
            else foreach (var t in path)
            {
                Logger.Log(t.ToString());
            }
        }
    }
}