using System;
using System.Collections.Generic;
using System.Linq;
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
        /// It will simply stop recursing deeper after a depth higher than <see cref="MaxDepth"/>.
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

            // In this case LINQ made it way more readable :) so we keep it
            // we only want to ask neighbors for a path, if they aren't in the path towards us
            // otherwise there would be some nasty infinite loops making this bad algorithm even worse
            foreach (var nb in graph.GetPassableConnections(start).Where(nb => !pathTo.Contains(nb)))
            {
                // end and start have to be flipped because the path is built in reverse order
                if (nb.Equals(end)) return new List<T> {end, start};

                // hand the task of finding a path off to the neighbor
                var newPath = RecursiveSolve(graph, nb, end, new List<T>(pathTo) {start}, depth + 1);

                // the new path is only better if it exists, if there was no previous path, or
                // if it is shorter than the previous path
                if (newPath.Count > 0 && (path == null || newPath.Count < path.Count))
                    path = newPath;
            }
            
            // If there is a path we want to add start to it (living off our neighbors work)
            path?.Add(start);

            return path ?? new List<T>();
        }
    }
}