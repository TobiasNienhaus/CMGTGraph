﻿using System;
using System.Text;
using CMGTGraph;
using CMGTGraph.Types;
using NUnit.Framework;

namespace CMGTGraphTest
{
    public class GraphTest
    {
        private Graph<Point> _graph;

        [SetUp]
        public void SetUp()
        {
            _graph = new Graph<Point>(Point.Calculator);
        }

        [Test]
        public void NullTests()
        {
            Assert.NotNull(_graph.Connections);
            Assert.NotNull(_graph.Calculator);
            Assert.IsTrue(_graph.Connections.Count == _graph.NodeCount);
        }

        [Test]
        public void AdditionAndRemovalTest()
        {
            var sb = new StringBuilder();
            sb.AppendLine("Before Test:");
            sb.AppendLine(_graph.ToString());

            sb.AppendLine("Testing valid addition: ");
            _graph.Add(new Point(10, 0));
            _graph.Add(new Point(9, 0));
            _graph.Add(new Point(8, 0));
            _graph.Add(new Point(7, 0));
            _graph.Add(new Point(6, 0));
            sb.AppendLine("After valid addition: ");
            sb.AppendLine(_graph.ToString());
            Assert.IsTrue(_graph.NodeCount == 5, "Wrong node count after valid addition ({0})",
                _graph.NodeCount.ToString());
            
            _graph.Add(new Point(10, 0));
            _graph.Add(new Point(9, 0));
            _graph.Add(new Point(8, 0));
            _graph.Add(new Point(7, 0));
            _graph.Add(new Point(6, 0));
            sb.AppendLine("After adding duplicates: ");
            sb.AppendLine(_graph.ToString());
            Assert.IsTrue(_graph.NodeCount == 5, "Wrong node count after duplicate addition ({0})",
                _graph.NodeCount.ToString());

            _graph.RemoveNode(new Point(10, 0));
            _graph.RemoveNode(new Point(9, 0));
            _graph.RemoveNode(new Point(8, 0));
            _graph.RemoveNode(new Point(7, 0));
            _graph.RemoveNode(new Point(6, 0));
            sb.AppendLine("After valid removal: ");
            sb.AppendLine(_graph.ToString());
            Assert.IsTrue(_graph.NodeCount == 0, "Wrong node count after valid removal ({0})",
                _graph.NodeCount.ToString());

            Assert.IsTrue(!_graph.RemoveNode(new Point(0, 1002)), "Wrong node count after invalid removal ({0})",
                _graph.NodeCount.ToString());

            Console.WriteLine(sb.ToString());
        }

        [Test]
        public void AdditionAndRemovalConnectionsTest()
        {
            var sb = new StringBuilder();
            sb.AppendLine("Before: ");
            sb.AppendLine(_graph.ToString());
            
            _graph.Add(new Point(0, 0));
            _graph.AddConnection(new Point(0, 0), new Point(0, 1));
            _graph.AddConnection(new Point(0, 0), new Point(0, 2));
            _graph.AddConnection(new Point(0, 0), new Point(0, 3));
            _graph.AddConnection(new Point(0, 0), new Point(0, 4));
            sb.AppendLine("After adding connections");
            sb.AppendLine(_graph.ToString());

            _graph.RemoveConnection(new Point(0, 0), new Point(0, 1));
            _graph.RemoveConnection(new Point(0, 0), new Point(0, 2));
            _graph.RemoveConnection(new Point(0, 0), new Point(0, 3));
            _graph.RemoveConnection(new Point(0, 0), new Point(0, 4));
            sb.AppendLine("After removing connections");
            sb.AppendLine(_graph.ToString());

            sb.AppendLine("Asserting removing connection with one invalid node");
            Assert.IsFalse(_graph.RemoveConnection(new Point(0, 0), new Point(99, 99)));
            Assert.IsFalse(_graph.RemoveConnection(new Point(99, 99), new Point(0, 0)));
            sb.AppendLine("Asserting removing with two invalid nodes");
            Assert.IsFalse(_graph.RemoveConnection(new Point(12, 12), new Point(99, 99)));

            sb.AppendLine("Graph after removal: ");
            Console.WriteLine(sb.ToString());
            sb.Clear();

            for (var i = 0; i < 5; i++)
            {
                var p = new Point(0, i);
                sb.AppendFormat("Asserting connection count is zero for {0}\n", p);
                Assert.IsTrue(_graph.GetConnections(p).Count == 0, "Graph still has connections ({0})", p);
            }

            sb.AppendLine("Asserting removing connection between points with no connection");
            Assert.IsFalse(_graph.RemoveConnection(new Point(0, 0), new Point(0, 1)));

            Console.WriteLine(sb.ToString());
        }

        [Test]
        public void GetConnectionsTest()
        {
            _graph.Add(new Point(0, 0));
            _graph.AddConnection(new Point(0, 0), new Point(0, 1));
            _graph.AddConnection(new Point(0, 0), new Point(0, 2));
            _graph.AddConnection(new Point(0, 0), new Point(0, 3));
            _graph.AddConnection(new Point(0, 0), new Point(0, 4));

            var sb = new StringBuilder();
            sb.AppendLine("Testing GetConnections");

            sb.AppendLine("Asserting that GetConnection returns correct number of connections");
            Assert.IsTrue(_graph.GetConnections(new Point(0, 0)).Count == 4,
                "GetConnections returned wrong number of connections.");

            var connections = _graph.GetConnections(new Point(0, 0));
            for (var i = 1; i < 5; i++)
            {
                var p = new Point(0, i);
                sb.AppendFormat("Asserting that connections contains: {0}\n", p);
                Assert.IsTrue(connections.Contains(p), "Point {0} isn't contained in the connections", p);
            }

            sb.AppendLine("Asserting that exception is thrown");
            Assert.Catch<Graph<Point>.NodeNotFoundException>(() => _graph.GetConnections(new Point(-9, -9)));

            Console.WriteLine(sb.ToString());
        }

        [Test]
        public void ContainsTest()
        {
            _graph.Add(new Point(0, 0));
            Assert.True(_graph.Contains(new Point(0, 0)));
            Assert.False(_graph.Contains(new Point(-1000, -82340)));
        }

        [Test]
        public void ToStringTest()
        {
            for (var i = 0; i < 10; i++)
            {
                _graph.Add(new Point(i, i));
            }
            Assert.NotNull(_graph.ToString());
        }
        
        [Test]
        public void ClearTest()
        {
            for (var i = 0; i < 100; i++)
            {
                _graph.Add(new Point(i, i));
            }
            _graph.Clear();
            
            Assert.True(_graph.NodeCount == 0);
        }

        [Test]
        public void HaveConnectionTest()
        {
            _graph.Add(new Point(0, 1));
            _graph.AddConnection(new Point(-2, -2), new Point(-1, -1));
            
            Assert.True(_graph.HaveConnection(new Point(-2, -2), new Point(-1, -1)));
            Assert.True(_graph.HaveConnection(new Point(-1, -1), new Point(-2, -2)));
            
            Assert.False(_graph.HaveConnection(new Point(0, 1), new Point(-2, -2)));
            Assert.False(_graph.HaveConnection(new Point(-2, -2), new Point(0, 1)));

            Assert.Catch<Graph<Point>.NodeNotFoundException>(() =>
                _graph.HaveConnection(new Point(-222, 1), new Point(0, 1)));
            Assert.Catch<Graph<Point>.NodeNotFoundException>(() =>
                _graph.HaveConnection(new Point(0, 1), new Point(-222, 1)));
            Assert.Catch<Graph<Point>.NodeNotFoundException>(() =>
                _graph.HaveConnection(new Point(-222, 1), new Point(1111, 1)));
        }
    }
}