using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using CMGTGraph.Calculators;
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
        public static List<T> AStarSolve<T>(this IReadOnlyGraph<T> graph, T start, T end)
            where T : IEquatable<T>
        {
            graph.ThrowOnInvalidInput(start, end);

            return graph.AStarSolveWithInfo(start, end).Path;
        }
        
        /// <summary>
        /// Get a path between two points in the graph using the A* algorithm.
        /// A list of the visited nodes is also returned. 
        /// If no path can be found, both lists in the result are empty.
        /// </summary>
        public static PathFindingResult<T> AStarSolveWithInfo<T>(this IReadOnlyGraph<T> g, T start, T end)
            where T : IEquatable<T>
        {
            g.ThrowOnInvalidInput(start, end);

            var closed = new HashSet<AStarNode<T>>();
            var open = new HashSet<AStarNode<T>>
                {new AStarNode<T>(start, distanceToFinish: g.Calculator.Distance(start, end))};

            while (open.Count > 0)
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
                {
                    return new PathFindingResult<T>(BuildPath(recordHolder, closed),
                        new HashSet<T>(open.Select(x => x.Data)),
                        new HashSet<T>(closed.Select(x => x.Data)));
                }

                closed.Add(recordHolder);
                AStarExpandNode(recordHolder, end, g.GetPassableConnections(recordHolder.Data), open, closed,
                    g.Calculator);
            }

            return PathFindingResult<T>.Empty;
        }

        private static void AStarExpandNode<T>(DijkstraNode<T> node, T finish, IEnumerable<T> neighbors,
            ICollection<AStarNode<T>> open, ICollection<AStarNode<T>> closed, ICalculator<T> calculator) where T : IEquatable<T>
        {
            foreach (var neighbor in neighbors)
            {
                var n = new AStarNode<T>(neighbor);
                if(closed.Contains(n)) continue;

                // TODO spam also F G and H when changing them
                
                // TODO i suspect this is not calculated correctly (diagonal is sometimes chosen over direct)
                var currentPathLength = node.CurrentPathLength + calculator.Distance(node.Data, neighbor);
                if (!open.Contains(n))
                {
                    n.Predecessor = node.Data;
                    n.CurrentPathLength = currentPathLength;
                    n.DistanceToFinish = calculator.Distance(neighbor, finish);
                    open.Add(n);
                    Logger.Spam($"Added new node {n.Data.ToString()} F {n.EstimatedCompletePathLength.ToString(CultureInfo.InvariantCulture)} G {n.CurrentPathLength.ToString(CultureInfo.InvariantCulture)} H {n.DistanceToFinish.ToString(CultureInfo.InvariantCulture)}");
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