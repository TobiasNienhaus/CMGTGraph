using System;
using System.Collections.Generic;
using System.Linq;
using CMGTGraph.Logging;

namespace CMGTGraph.Algorithms
{
    public static partial class Algorithms
    {
        #region Node
        public class AStarNode<T> : DijkstraNode<T> where T : IEquatable<T>
        {
            public float DistanceToFinish;
            public float EstimatedCompletePathLength => CurrentPathLength + DistanceToFinish;

            public AStarNode(T data, T predecessor = default, float currentPathLength = 0f,
                float distanceToFinish = float.MaxValue) : base(data, predecessor, currentPathLength)
            {
                DistanceToFinish = distanceToFinish;
            }
        }
        #endregion


        /// <summary>
        /// Get a path between two points in the graph using the A* algorithm.
        /// If no path can be found, an empty list is returned.
        /// </summary>
        /// <exception cref="Graph{T}.NodeNotFoundException">Thrown when one (or both) of the nodes can't be found in the
        /// graph</exception>
        public static List<T> AStarSolve<T>(this IReadOnlyGraph<T> graph, T start, T end)
            where T : IEquatable<T>
        {
            graph.ThrowOnInvalidInput(start, end);
            
            // var closed = new Dictionary<T, Node<T>>();
            var closed = new HashSet<AStarNode<T>>();
            // var open = new Dictionary<T, Node<T>>();
            var open = new List<AStarNode<T>> {new AStarNode<T>(start, distanceToFinish: graph.Calculator.Distance(start, end))};

            var connections = graph.Connections;

            while(open.Count > 0)
            {
                // get most promising node
                var recordHolder = open.First();
                var record = recordHolder.EstimatedCompletePathLength;
                foreach (var node in open)
                {
                    if (node.EstimatedCompletePathLength >= record) continue;
                    record = node.EstimatedCompletePathLength;
                    recordHolder = node;
                }

                open.Remove(recordHolder);

                if (recordHolder.Data.Equals(end))
                    return BuildPath(recordHolder, closed);

                closed.Add(recordHolder);
                AStarExpandNode(recordHolder, end, connections[recordHolder.Data], open, closed, graph.Calculator);
            }

            return new List<T>();
        }

        private static void AStarExpandNode<T>(DijkstraNode<T> node, T finish, HashSet<T> neighbors,
            ICollection<AStarNode<T>> open, ICollection<AStarNode<T>> closed, ICalculator<T> calculator) where T : IEquatable<T>
        {
            foreach (var neighbor in neighbors)
            {
                var n = new AStarNode<T>(neighbor);
                if(closed.Contains(n)) continue;

                var currentPathLength = node.CurrentPathLength + calculator.Distance(node.Data, neighbor);
                if (!open.Contains(n))
                {
                    n.Predecessor = node.Data;
                    n.CurrentPathLength = currentPathLength;
                    n.DistanceToFinish = calculator.Distance(neighbor, finish);
                    open.Add(n);
                    Logger.Spam($"Added new node {n.Data.ToString()}");
                }
                else if(currentPathLength < n.CurrentPathLength)
                {
                    n.CurrentPathLength = currentPathLength;
                    n.Predecessor = node.Data;
                    Logger.Spam($"Reevaluated {n.Data.ToString()}");
                }
            }
        }
    }
}