using System;
using System.Collections.Generic;
using System.Linq;
using CMGTGraph.Logging;

namespace CMGTGraph.Algorithms
{
    public static partial class Algorithms
    {
        public class DijkstraNode<T> : Node<T> where T : IEquatable<T>
        {
            public float CurrentPathLength;
            
            /// <inheritdoc />
            public DijkstraNode(T data, T predecessor = default, float currentPathLength = float.MaxValue) : base(data, predecessor)
            {
                CurrentPathLength = currentPathLength;
            }
        }

        public static List<T> DijkstraSolve<T>(this IReadOnlyGraph<T> graph, T start, T end) where T : IEquatable<T>
        {
            graph.ThrowOnInvalidInput(start, end);
            
            var startNode = new DijkstraNode<T>(start, currentPathLength: 0f);
            var open = new List<DijkstraNode<T>> {startNode};

            var visited = new HashSet<DijkstraNode<T>> {startNode};

            while (open.Count > 0)
            {
                // get the node that has the shortest saved path to the start node
                var curr = open.First();
                var record = curr.CurrentPathLength;
                foreach (var node in open)
                {
                    if (node == curr || node.CurrentPathLength >= record) continue;
                    record = node.CurrentPathLength;
                    curr = node;
                }

                if (curr.Data.Equals(end))
                    return BuildPath(curr, visited);

                open.Remove(curr);
                visited.Add(curr);
                DijkstraExpandNode(curr, graph.GetConnections(curr.Data), open, visited, graph.Calculator);
            }
            Logger.Warn("No path found!");
            return new List<T>();
        }

        private static void DijkstraExpandNode<T>(DijkstraNode<T> node, HashSet<T> neighbors,
            ICollection<DijkstraNode<T>> open, ICollection<DijkstraNode<T>> closed, ICalculator<T> calculator)
            where T : IEquatable<T>
        {
            foreach (var nb in neighbors)
            {
                var n = new DijkstraNode<T>(nb);
                if(closed.Contains(n)) continue;

                var currentPathLength = node.CurrentPathLength + calculator.Distance(node.Data, nb);
                if (!open.Contains(n))
                {
                    n.Predecessor = node.Data;
                    n.CurrentPathLength = currentPathLength;
                    open.Add(n);
                    Logger.Spam($"Added new node: {n.Data.ToString()}");
                }
                else if(currentPathLength < n.CurrentPathLength)
                {
                    n.CurrentPathLength = currentPathLength;
                    n.Predecessor = n.Data;
                    Logger.Spam($"Reevaluated {n.Data.ToString()}");
                }
            }
        }
    }
}