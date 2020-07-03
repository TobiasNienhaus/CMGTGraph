using System;
using System.Collections.Generic;
using System.Linq;
using CMGTGraph.Logging;

namespace CMGTGraph.Algorithms
{
    public static partial class Algorithms
    {
        /// <summary>
        /// Use the iterative bfs algorithm to find a path between <paramref name="start"/> and <paramref name="end"/>.
        /// </summary>
        /// <param name="graph">The graph to solve on</param>
        /// <param name="start">The start node</param>
        /// <param name="end">The end node</param>
        /// <exception cref="Graph{T}.NodeNotFoundException">This will be thrown if either
        /// start or end are in the graph.</exception>
        /// <exception cref="InvalidOperationException">This will be thrown when one of
        /// the nodes is not passable at the time of asking (politely of course)</exception>
        public static List<T> IterativeBfsSolve<T>(this IReadOnlyGraph<T> graph, T start, T end) where T : IEquatable<T>
        {
            graph.ThrowOnInvalidInput(start, end);

            return graph.IterativeBfsSolveWithInfo(start, end).Path;
        }

        /// <summary>
        /// Use iterative BFS to find a path between <paramref name="start"/> and <paramref name="end"/>.
        /// This method will also return the visited nodes in the process.
        /// <br/>The returned <see cref="PathFindingResult{T}"/> will contain the path
        /// (in <see cref="PathFindingResult{T}.Path"/>), and the visited nodes
        /// (in <see cref="PathFindingResult{T}.OpenNodes"/>).
        /// <see cref="PathFindingResult{T}.ClosedNodes"/> will be empty as that is not applicable here.
        /// </summary>
        /// <param name="graph">The graph to operate on</param>
        /// <param name="start">The node to start searching from</param>
        /// <param name="end">The node that is the finish</param>
        public static PathFindingResult<T> IterativeBfsSolveWithInfo<T>(this IReadOnlyGraph<T> graph, T start, T end)
            where T : IEquatable<T>
        {
            // in BFS nodes are evaluated in order of appearance, so a Queue makes sense
            var nodesToCheck = new Queue<Node<T>>();
            nodesToCheck.Enqueue(new Node<T>(start));
            
            // These will be the visited nodes so we don't visit stuff multiple times (no loops either)
            var visited = new HashSet<Node<T>> {new Node<T>(start)};

            while (nodesToCheck.Count > 0)
            {
                var currentNode = nodesToCheck.Dequeue();
                var neighbors = graph.GetPassableConnections(currentNode.Data);
                foreach (var nb in neighbors)
                {
                    // create a dummy node to check if it is the end and if we need to add it.
                    // This works (also Contains()), because we have overriden Equals() and GetHashCode() accordingly
                    var node = new Node<T>(nb, currentNode.Data);
                    if (nb.Equals(end))
                    {
                        return new PathFindingResult<T>(BuildPath(node, visited), 
                            new HashSet<T>(visited.Select(x => x.Data)),
                            new HashSet<T>());
                    }

                    // Add() returns true, if the element is successfully added (no duplicates because hashset) :)
                    if(visited.Add(node))
                        nodesToCheck.Enqueue(node);
                }
            }

            // TODO should we warn?
            Logger.Warn("No path found!");
            return PathFindingResult<T>.Empty;
        }
    }
}