using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using CMGTGraph;
using CMGTGraph.Algorithms;
using CMGTGraph.Types;

namespace CMGTGraphTest
{
    public class AStarTestsPointF
    {
        private Graph<PointF> _testGraph;
        
        [SetUp]
        public void Setup()
        {
            Console.WriteLine($"Testing with {nameof(PointF)}");

            _testGraph = new Graph<PointF>(PointF.Calculator);
            _testGraph.Add(new PointF(0f, 1f));
            _testGraph.AddConnection(new PointF(0f, 1f), new PointF(1f, 2f));
            _testGraph.AddConnection(new PointF(2f, 3f), new PointF(3f, 4f));
            _testGraph.AddConnection(new PointF(3f, 4f), new PointF(1f, 2f));
            // separate net
            _testGraph.AddConnection(new PointF(800f, 100f), new PointF(900f, 100f));
            _testGraph.AddConnection(new PointF(900f, 100f), new PointF(850f, 1000f));
            _testGraph.AddConnection(new PointF(800f, 100f), new PointF(850f, 1000f));
            _testGraph.AddConnection(new PointF(850f, 1000f), new PointF(950f, 800f));
            _testGraph.AddConnection(new PointF(950f, 800f), new PointF(20f, 80f));
            Console.WriteLine("Setup Graph for testing");
            Console.Write(_testGraph.ToString());
            Console.WriteLine("Finished Setup\n");
        }

        [Test, Timeout(5000)]
        public void TestValidAStarInputNodes()
        {
            Console.WriteLine("Testing a valid request to solve with A*");
            Console.WriteLine("-------------");

            var path = _testGraph.AStarSolve(new PointF(0, 1), new PointF(3, 4));
            Assert.IsNotEmpty(path, "Path was empty after passing valid points to AStarSolve!");
            PrintPath(path);
            Console.WriteLine("-------------");

            var path2 = _testGraph.AStarSolve(new PointF(800, 100), new PointF(20, 80));
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
            var path = _testGraph.AStarSolve(new PointF(-100f, -100f), new PointF(-900f, -900f));
            Assert.IsEmpty(path, "Path wasn't empty after passing invalid start and end to AStarSolve!");

            Console.WriteLine("-------------");
            var path2 = _testGraph.AStarSolve(new PointF(-100f, -100f), new PointF(950f, 800f));
            Assert.IsEmpty(path2, "Path wasn't empty after passing invalid start to AStarSolve!");

            Console.WriteLine("-------------");
            var path3 = _testGraph.AStarSolve(new PointF(950f, 800f), new PointF(-100f, -100f));
            Assert.IsEmpty(path3, "Path wasn't empty after passing invalid end to AStarSolve!");

            Console.WriteLine("-------------");
            Console.WriteLine("Finished testing invalid A* input nodes (start and/or end don't exist in graph)");
        }

        [Test, Timeout(5000)]
        public void TestAStarNoPath()
        {
            Console.WriteLine("Testing result if no path can be found");
            
            var path = _testGraph.AStarSolve(new PointF(0f, 1f), new PointF(950f, 800f));
            Assert.IsEmpty(path, "Path wasn't empty, even though there couldn't have been a path!");

            Console.WriteLine("Finished testing result if no path can be found");
        }

        private static void PrintPath(List<PointF> points)
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