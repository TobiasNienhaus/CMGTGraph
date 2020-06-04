using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using CMGTGraph;
using CMGTGraph.Algorithms;
using CMGTGraph.Types;

namespace CMGTGraphTest
{
    public class CommonAStarOperationsTest
    {
        private Graph<Point> _testGraph;
        
        [SetUp]
        public void Setup()
        {
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
        }

        [Test]
        public void InvalidNodeAsArgumentTest()
        {
            Assert.Catch<Graph<Point>.NodeNotFoundException>(() =>
                    _testGraph.AStarSolve(new Point(-100, -100), new Point(-299, -299)),
                "NodeNotFoundException wasn't thrown, even though both of the provided values couldn't be found");

            Assert.Catch<Graph<Point>.NodeNotFoundException>(() =>
                    _testGraph.AStarSolve(new Point(-100, -100), new Point(950, 800)),
                "NodeNotFoundException wasn't thrown, even though one of the provided values couldn't be found");

            Assert.Catch<Graph<Point>.NodeNotFoundException>(() =>
                    _testGraph.AStarSolve(new Point(950, 800), new Point(-100, -100)),
                "NodeNotFoundException wasn't thrown, even though one of the provided values couldn't be found");

            Assert.Catch(() =>
                    _testGraph.AStarSolve(null, new Point(-100, -100)),
                "No Exception was thrown, even though one of the provided values was null");

            Assert.Catch(() =>
                    _testGraph.AStarSolve(new Point(950, 800), null),
                "No Exception was thrown, even though one of the provided values was null");
        }

        [Test, Timeout(25)]
        public void ValidPathTest()
        {
            var path = _testGraph.AStarSolve(new Point(0, 1), new Point(3, 4));
            Assert.IsNotEmpty(path, "Path was empty, even though there was a path.");
        }

        [Test, Timeout(25)]
        public void NoPathFoundException()
        {
            var path = _testGraph.AStarSolve(new Point(0, 1), new Point(950, 800));
            Assert.IsEmpty(path, "Returned path wasn't empty, even though there was no possible path.");
        }
    }
}