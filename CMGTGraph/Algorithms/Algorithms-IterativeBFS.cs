using System;
using System.Collections.Generic;
using System.Linq;
using CMGTGraph.Logging;

namespace CMGTGraph.Algorithms
{
    public static partial class Algorithms
    {
        public static List<T> IterativeBfsSolve<T>(this IReadOnlyGraph<T> graph, T start, T end) where T : IEquatable<T>
        {
            graph.ThrowOnInvalidInput(start, end);

            return graph.IterativeBfsSolveWithInfo(start, end).Path;
        }

        public static PathFindingResult<T> IterativeBfsSolveWithInfo<T>(this IReadOnlyGraph<T> graph, T start, T end)
            where T : IEquatable<T>
        {
            var queue = new Queue<Node<T>>();
            queue.Enqueue(new Node<T>(start));

            var visited = new HashSet<Node<T>> {new Node<T>(start)};

            while (queue.Count > 0)
            {
                var curr = queue.Dequeue();
                var nbs = graph.GetPassableConnections(curr.Data);
                foreach (var nb in nbs)
                {
                    var node = new Node<T>(nb, curr.Data);
                    if (nb.Equals(end))
                    {
                        return new PathFindingResult<T>(BuildPath(node, visited), 
                            new HashSet<T>(visited.Select(x => x.Data)),
                            new HashSet<T>());
                    }

                    if (visited.Contains(node)) continue;

                    visited.Add(node);
                    queue.Enqueue(node);
                }
            }

            Logger.Warn("No path found!");
            return PathFindingResult<T>.Empty;
        }
    }
}