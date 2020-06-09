using CMGTGraph;
using CMGTGraph.Algorithms;
using CMGTGraph.Logging;
using CMGTGraph.Types;
using NUnit.Framework;

namespace CMGTGraphTest.AlgorithmsTests.Dijkstra
{
    [TestFixture]
    public class FunctionalityTest
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
            
            _testGraph.AddConnection(new Point(3, 4), new Point(10, 40));
            _testGraph.AddConnection(new Point(10, 40), new Point(20, 80));
            _testGraph.MakeImpassable(new Point(10, 40));
            
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
                    _testGraph.DijkstraSolve(new Point(-100, -100), new Point(-299, -299)),
                "NodeNotFoundException wasn't thrown, even though both of the provided values couldn't be found");

            Assert.Catch<Graph<Point>.NodeNotFoundException>(() =>
                    _testGraph.DijkstraSolve(new Point(-100, -100), new Point(950, 800)),
                "NodeNotFoundException wasn't thrown, even though one of the provided values couldn't be found");

            Assert.Catch<Graph<Point>.NodeNotFoundException>(() =>
                    _testGraph.DijkstraSolve(new Point(950, 800), new Point(-100, -100)),
                "NodeNotFoundException wasn't thrown, even though one of the provided values couldn't be found");

            Assert.Catch(() =>
                    _testGraph.DijkstraSolve(null, new Point(-100, -100)),
                "No Exception was thrown, even though one of the provided values was null");

            Assert.Catch(() =>
                    _testGraph.DijkstraSolve(new Point(950, 800), null),
                "No Exception was thrown, even though one of the provided values was null");
        }

        [Test, Timeout(25)]
        public void ValidPathTest()
        {
            var path = _testGraph.DijkstraSolve(new Point(0, 1), new Point(3, 4));
            Assert.IsNotEmpty(path, "Path was empty, even though there was a path.");
        }

        [Test, Timeout(25)]
        public void NoPathFoundException()
        {
            var path = _testGraph.DijkstraSolve(new Point(0, 1), new Point(950, 800));
            Assert.IsEmpty(path, "Returned path wasn't empty, even though there was no possible path.");
        }
    }
}