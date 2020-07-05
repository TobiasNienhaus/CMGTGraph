using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using CMGTGraph;
using CMGTGraph.Algorithms;
using CMGTGraph.Calculators;
using CMGTGraph.Logging;
using CMGTGraph.Types;
using NUnit.Framework;

namespace CMGTGraphTest.AlgorithmsTests.AStar
{
    public class PerformanceTest
    {
        private Graph<Point> _g;
        private Random _random;
        
        [SetUp]
        public void Setup()
        {
            _g = new Graph<Point>(PointCalculator.This);
            _random = new Random();

            while (_g.NodeCount < 1000) _g.Add(new Point(_random.Next(100), _random.Next(100)));

            var nodes = _g.Nodes.ToArray();
            Logger.Log($"NodeCount: {nodes.Length.ToString()}");
            Console.WriteLine($"NodeCount: {nodes.Length.ToString()}");
            for (var i = 0; i < nodes.Length * 3; i++)
            {
                var n1 = nodes[_random.Next(0, nodes.Length)];
                var n2 = nodes[_random.Next(0, nodes.Length)];
                _g.AddConnection(n1, n2);
            }

            while (!_g.IsConnected())
            {
                var n1 = nodes[_random.Next(0, nodes.Length)];
                var n2 = nodes[_random.Next(0, nodes.Length)];
                _g.AddConnection(n1, n2);
            }
        }

        [Test]
        public void Test()
        {
            var logLevel = Logger.LoggingLevel;
            Logger.LoggingLevel = Logger.LogLevel.Verbose;
            
            var nodes = _g.Nodes.ToArray();
            var start = nodes[_random.Next(0, nodes.Length)];
            var end = nodes[_random.Next(0, nodes.Length)];

            var stopwatch = new Stopwatch();
            stopwatch.Start();
            
            var path = _g.AStarSolve(start, end);
            
            stopwatch.Stop();
            var elapsed = stopwatch.ElapsedMilliseconds;
            Logger.Log($"A* took {stopwatch.ElapsedTicks.ToString()} ticks");
            Logger.Log($"A* took {elapsed.ToString()}ms");
            Assert.LessOrEqual(elapsed, 250L, "Test took to long");
            Assert.IsNotEmpty(path);
            PrintPath(path);
            Logger.LoggingLevel = logLevel;

            Console.WriteLine(_g.ToString());
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