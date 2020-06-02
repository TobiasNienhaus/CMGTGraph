using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using CMGTGraph;
using CMGTGraph.Algorithms;
using CMGTGraph.Types;

namespace CMGTGraphTest
{
    public class AStarTestsPoint
    {
        private Graph<Point> _testGraph;
        
        [SetUp]
        public void Setup()
        {
            Console.WriteLine($"Testing with {nameof(Point)}");
            
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
            Console.WriteLine("Setup Graph for testing");
            Console.Write(_testGraph.ToString());
            Console.WriteLine("Finished Setup\n");
        }

        [Test, Timeout(5000)]
        public void TestValidAStarInputNodes()
        {
            Console.WriteLine("Testing a valid request to solve with A*");
            Console.WriteLine("-------------");
            
            var path = _testGraph.AStarSolve(new Point(0, 1), new Point(3, 4));
            Assert.IsNotEmpty(path, "Path was empty after passing valid points to AStarSolve!");
            PrintPath(path);
            Console.WriteLine("-------------");
            
            var path2 = _testGraph.AStarSolve(new Point(800, 100), new Point(20, 80));
            Assert.IsNotEmpty(path2, "Path was empty after passing valid points to AStarSolve!");
            PrintPath(path2);
            Console.WriteLine("-------------");

            Console.WriteLine("Finished testing a valid request to solve with A*");
        }
        
        [Test, Timeout(5000)]
        public void TestInvalidAStarInputNodes()
        {
            Console.WriteLine("Testing invalid A* input nodes (start and/or end don't exist in graph)");
            Console.WriteLine("-------------");
            var path = _testGraph.AStarSolve(new Point(-100, -100), new Point(-900, -900));
            Assert.IsEmpty(path, "Path wasn't empty after passing invalid start and end to AStarSolve!");

            Console.WriteLine("-------------");
            var path2 = _testGraph.AStarSolve(new Point(-100, -100), new Point(950, 800));
            Assert.IsEmpty(path2, "Path wasn't empty after passing invalid start to AStarSolve!");

            Console.WriteLine("-------------");
            var path3 = _testGraph.AStarSolve(new Point(950, 800), new Point(-100, -100));
            Assert.IsEmpty(path3, "Path wasn't empty after passing invalid end to AStarSolve!");

            Console.WriteLine("-------------");
            Console.WriteLine("Finished testing invalid A* input nodes (start and/or end don't exist in graph)");
        }

        [Test, Timeout(5000)]
        public void TestAStarNoPath()
        {
            Console.WriteLine("Testing result if no path can be found");
            
            var path = _testGraph.AStarSolve(new Point(0, 1), new Point(950, 800));
            Assert.IsEmpty(path, "Path wasn't empty, even though there couldn't have been a path!");

            Console.WriteLine("Finished testing result if no path can be found");
        }

        private static void PrintPath(List<Point> points)
        {
            var sb = new StringBuilder();
            sb.AppendLine("Path: ");
            foreach (var p in points)
            {
                sb.AppendLine(p.ToString());
            }

            Console.WriteLine(sb.ToString());
        }
    }
}