using System;
using System.Collections.Generic;
using CMGTGraph.Logging;

namespace CMGTGraph.Algorithms
{
    public static partial class Algorithms
    {
        private const int MaxDepth = 100;
        
        public static List<T> RecursiveSolve<T>(this IReadOnlyGraph<T> graph, T start, T end) where T : IEquatable<T>
        {
            if (!graph.Contains(start))
            {
                Logger.Error("Start node doesn't exist in graph!");
                throw new Graph<T>.NodeNotFoundException(start);
            }

            if (!graph.Contains(end))
            {
                Logger.Error("End node doesn't exist in graph!");
                throw new Graph<T>.NodeNotFoundException(end);
            }
            
            Logger.Warn("This can take a while!");
            
            // Algorithm:
            // RecursiveSolve(Graph, start, end, from, pathTo)
            // get neighbors of start node
            // Get recursive path from neighbor to end (if the neighbor is not in pathTo)
            // return shortest of the paths

            var path = graph.RecursiveSolve(start, end, new List<T>());
            path.Reverse();
            return path;
        }

        private static List<T> RecursiveSolve<T>(this IReadOnlyGraph<T> graph, T start, T end, List<T> pathTo,int depth = 0)
            where T : IEquatable<T>
        {
            if(depth == MaxDepth) return new List<T>();
            List<T> l = null;

            foreach (var c in graph.GetConnections(start))
            {
                if (pathTo.Contains(c)) continue;

                // end and start have to be flipped because the path is built in reverse order
                if (c.Equals(end))
                {
                    return new List<T> {end, start};
                }

                var newPath = RecursiveSolve(graph, c, end, new List<T>(pathTo) {start}, depth + 1);

                if (newPath.Count > 0 && (l == null || newPath.Count < l.Count))
                    l = newPath;
            }
            
            l?.Add(start);

            return l ?? new List<T>();
        }
    }
}