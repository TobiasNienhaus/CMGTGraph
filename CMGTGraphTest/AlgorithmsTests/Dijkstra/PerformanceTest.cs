using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using CMGTGraph;
using CMGTGraph.Algorithms;
using CMGTGraph.Calculators;
using CMGTGraph.Logging;
using CMGTGraph.Types;
using NUnit.Framework;

namespace CMGTGraphTest.AlgorithmsTests.Dijkstra
{
    [TestFixture]
    public class PerformanceTest
    {
        private Graph<Point> _g;
        private Random _random;

        [SetUp]
        public void Setup()
        {
            _g = new Graph<Point>(PointCalculator.This);
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

            var sw = new Stopwatch();
            sw.Start();
            var result = _g.DijkstraSolveWithInfo(start, end);
            sw.Stop();
            Logger.Error($"Dijkstra took {sw.ElapsedMilliseconds}ms, put {result.ClosedNodes.Count} on " +
                         $"its closed list, and put {result.OpenNodes.Count} on its open list. " +
                         $"(The whole graph has {_g.NodeCount} nodes)");
            Assert.IsNotEmpty(result.Path);
            PrintPath(result.Path);
        }

        private static void PrintPath(IReadOnlyCollection<Point> path)
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