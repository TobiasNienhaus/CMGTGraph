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
            graph.ThrowOnInvalidInput(start, end);
            
            Logger.Warn("This can take a while!");
            
            var path = RecursiveSolve(graph, start, end, new List<T>());
            path.Reverse();
            return path;
        }

        private static List<T> RecursiveSolve<T>(IReadOnlyGraph<T> graph, T start, T end, List<T> pathTo,int depth = 0)
            where T : IEquatable<T>
        {
            if(depth == MaxDepth) return new List<T>();
            List<T> l = null;

            foreach (var c in graph.GetPassableConnections(start))
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