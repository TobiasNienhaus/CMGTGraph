using System;
using System.Collections.Generic;
using System.Text;
using CMGTGraph;
using CMGTGraph.Algorithms;
using CMGTGraph.Types;
using NUnit.Framework;

namespace CMGTGraphTest.AlgorithmsTests.IterativeBfsSolve
{
    [TestFixture]
    public class PerformanceTest
    {
        private Graph<Point> _g;
        private Random _random;

        [SetUp]
        public void Setup()
        {
            _g = new Graph<Point>(Point.Calculator);
            _random = new Random();

            for (var x = 0; x < 100; x++)
            {
                for (var y = 0; y < 100; y++)
                {
                    var p = new Point(x, y);
                    if (x + 1 < 100)
                    {
                        var pr = new Point(x + 1, y);
                        _g.AddConnection(p, pr);
                    }

                    if (y + 1 < 100)
                    {
                        var pd = new Point(x, y + 1);
                        _g.AddConnection(p, pd);
                    }
                }
            }
        }

        [Test, MaxTime(5000)]
        public void Test()
        {
            var start = new Point(_random.Next(100), _random.Next(100));
            var end = new Point(_random.Next(100), _random.Next(100));

            var path = _g.AStarSolve(start, end);
            Assert.IsNotEmpty(path);
            Console.WriteLine($"Path from {start} to {end}");
            PrintPath(path);
        }

        private void PrintPath(List<Point> path)
        {
            var sb = new StringBuilder();
            sb.AppendFormat("Path: (SqrLength: {0})\n", path.Count.ToString());
            foreach (var point in path)
            {
                sb.AppendLine(point.ToString());
            }

            Console.WriteLine(sb.ToString());
        }
    }
}