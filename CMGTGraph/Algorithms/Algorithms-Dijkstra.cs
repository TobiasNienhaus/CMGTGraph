﻿using System;
using System.Collections.Generic;
using System.Linq;
using CMGTGraph.Calculators;
using CMGTGraph.Logging;

namespace CMGTGraph.Algorithms
{
    public static partial class Algorithms
    {
        /// <summary>
        /// A node specifically tailored to the dijkstra algorithm with a predecessor and
        /// information about the current path length to this node.
        /// </summary>
        public class DijkstraNode<T> : Node<T> where T : IEquatable<T>
        {
            /// <summary>
            /// The length of the path to this node.
            /// </summary>
            public float CurrentPathLength;
            
            /// <inheritdoc />
            public DijkstraNode(T data, T predecessor = default, float currentPathLength = float.MaxValue) : base(data, predecessor)
            {
                CurrentPathLength = currentPathLength;
            }
        }

        public static List<T> DijkstraSolve<T>(this IReadOnlyGraph<T> graph, T start, T end) where T : IEquatable<T>
        {
            // not strictly necessary (DijkstraSolveWithInfo does the same), bu oh well, better safe than sorry :)
            graph.ThrowOnInvalidInput(start, end);

            return graph.DijkstraSolveWithInfo(start, end).Path;
        }

        public static PathFindingResult<T> DijkstraSolveWithInfo<T>(this IReadOnlyGraph<T> g, T start, T end)
            where T : IEquatable<T>
        {
            g.ThrowOnInvalidInput(start, end);

            var startNode = new DijkstraNode<T>(start, currentPathLength: 0f);
            // we have visited the start node already and thus don't need to again
            var visited = new HashSet<DijkstraNode<T>> {startNode};
            // we also want to add it to the open set though, to have a node to evaluate the neighbors from
            var open = new HashSet<DijkstraNode<T>> {startNode};

            while (open.Count > 0)
            {
                // get the node that has the shortest saved path to the start node
                // TODO refactor into function (Finding best node)
                var curr = open.First();
                var record = curr.CurrentPathLength;
                foreach (var node in open)
                {
                    if (node == curr || node.CurrentPathLength >= record) continue;
                    record = node.CurrentPathLength;
                    curr = node;
                }

                // if the best node is actually the finish: hooray lucky day, return it
                if (curr.Data.Equals(end))
                {
                    return new PathFindingResult<T>(BuildPath(curr, visited),
                        new HashSet<T>(open.Select(x => x.Data)),
                        new HashSet<T>(visited.Select(x => x.Data)));
                }

                // the just evaluated node isn't open for evaluation again, we were to pushy :(
                open.Remove(curr);
                // we have in fact just visited it
                visited.Add(curr);
                // expand this node
                DijkstraExpandNode(curr, g.GetPassableConnections(curr.Data), open, visited, g.Calculator);
            }
            // we haven't found a path
            return PathFindingResult<T>.Empty;
        }

        /// <summary>
        /// Expand a node, which basically means to check if any of the neighbors need their values changed
        /// (or initialized) and, if they aren't already, the eligible neighbors will be added to the waiting list,
        /// waiting to be evaluated.
        /// </summary>
        /// <param name="node">The node to check the neighbors of</param>
        /// <param name="neighbors">The neighbors of this node</param>
        /// <param name="open">The list of nodes that are already queued for evaluation</param>
        /// <param name="closed">The list of nodes that is finished with this shit</param>
        /// <param name="calculator">The calculator to use for distance calculation</param>
        private static void DijkstraExpandNode<T>(DijkstraNode<T> node, HashSet<T> neighbors,
            ICollection<DijkstraNode<T>> open, ICollection<DijkstraNode<T>> closed, ICalculator<T> calculator)
            where T : IEquatable<T>
        {
            foreach (var nb in neighbors)
            {
                var n = new DijkstraNode<T>(nb);
                // TODO same as with A*, we don't actually get the values from the open/closed list
                // -> they will be the default values
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