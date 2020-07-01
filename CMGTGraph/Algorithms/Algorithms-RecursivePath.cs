using System;
using System.Collections.Generic;
using CMGTGraph.Logging;

namespace CMGTGraph.Algorithms
{
    public static partial class Algorithms
    {
        private const int MaxDepth = 100;
        
        /// <summary>
        /// Kind of the worst kind of pathfinding you can choose, ever.
        /// <br/>It will (eventually) return a path between <see cref="start"/> and <see cref="end"/> using a
        /// recursive algorithm.
        /// </summary>
        public static List<T> RecursiveSolve<T>(this IReadOnlyGraph<T> graph, T start, T end) where T : IEquatable<T>
        {
            graph.ThrowOnInvalidInput(start, end);
            
            Logger.Warn("This can take a while!");
            
            var path = RecursiveSolve(graph, start, end, new List<T>());
            path.Reverse();
            return path;
        }

        /// <summary>
        /// The actual recursive method that is used to calculate the path.
        /// <br/>This method has an "artificial" recursion anchor at <see cref="MaxDepth"/> to
        /// prevent a <see cref="StackOverflowException"/>.
        /// It will simply stop recursing deeper after a depth deeper than <see cref="MaxDepth"/>.
        /// <br/>Use cautiously :) it can take a while.
        /// </summary>
        /// <param name="graph">The graph to perform the operation on.</param>
        /// <param name="start">The start node of the problem</param>
        /// <param name="end">The end node.</param>
        /// <param name="pathTo">The current path to the start node.</param>
        /// <param name="depth">The current depth we are at.</param>
        /// <typeparam name="T"></typeparam>
        /// <returns>Returns the reverse path from <see cref="start"/> to <see cref="end"/></returns>
        private static List<T> RecursiveSolve<T>(IReadOnlyGraph<T> graph, T start, T end, List<T> pathTo, int depth = 0)
            where T : IEquatable<T>
        {
            if(depth == MaxDepth) return new List<T>();
            List<T> path = null;

            foreach (var nb in graph.GetPassableConnections(start))
            {
                if (pathTo.Contains(nb)) continue;

                // end and start have to be flipped because the path is built in reverse order
                if (nb.Equals(end))
                {
                    return new List<T> {end, start};
                }

                var newPath = RecursiveSolve(graph, nb, end, new List<T>(pathTo) {start}, depth + 1);

                if (newPath.Count > 0 && (path == null || newPath.Count < path.Count))
                    path = newPath;
            }
            
            path?.Add(start);

            return path ?? new List<T>();
        }
    }
}