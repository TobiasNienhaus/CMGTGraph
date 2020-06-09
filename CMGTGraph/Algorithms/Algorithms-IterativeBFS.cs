using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using CMGTGraph.Logging;

namespace CMGTGraph.Algorithms
{
    public static partial class Algorithms
    {
        public static List<T> IterativeBfsSolve<T>(this IReadOnlyGraph<T> graph, T start, T end) where T : IEquatable<T>
        {
            graph.ThrowOnInvalidInput(start, end);
            
            var q = new Queue<Node<T>>();
            q.Enqueue(new Node<T>(start));

            var visited = new HashSet<Node<T>> {new Node<T>(start)};

            while (q.Count > 0)
            {
                var curr = q.Dequeue();
                var nbs = graph.GetConnections(curr.Data);
                foreach (var nb in nbs)
                {
                    var node = new Node<T>(nb, curr.Data);
                    if (nb.Equals(end))
                    {
                        return BuildPath(node, visited);
                    }
                    
                    if(visited.Contains(node)) continue;

                    visited.Add(node);
                    q.Enqueue(node);
                }
            }
            Logger.Warn("No path found!");
            return new List<T>();
        }
    }
}