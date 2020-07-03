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
        /// <summary>
        /// A Node specifically built for the purposes of the A* algorithm.
        /// It saves the current data it represents (the thing from the graph), 
        /// the predecessor of this node in the current run of the algorithm,
        /// the accumulated path length to this node currently, 
        /// the estimated distance from this node to the finish and 
        /// an option to get the estimated complete path length going through this node.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public class AStarNode<T> : DijkstraNode<T> where T : IEquatable<T>
        {
            /// <summary>
            /// The estimated distance of this node to the finish.
            /// In formal definitions this is often called H or h(x) standing for heuristic.
            /// </summary>
            public float DistanceToFinish;
            
            /// <summary>
            /// The estimated complete path length that would go through this node.
            /// It is the length to this node (<see cref="DijkstraNode{T}.CurrentPathLength"/>) plus 
            /// the estimated distance to the finish.
            /// In formal definitions, this is often called F or f(x) and defined as f(x) = g(x) + h(x)
            /// </summary>
            public float EstimatedCompletePathLength => CurrentPathLength + DistanceToFinish;

            /// <summary>
            /// Create a new AStarNode. You don't need to specify any values apart from the actual data.
            /// They can all be set later. <paramref name="predecessor"/> will by default 
            /// initialize to <code>default</code>, <paramref name="currentPathLength"/> will default to zero and
            /// <paramref name="distanceToFinish"/> will default to <see cref="float.MaxValue"/>.
            /// </summary>
            public AStarNode(T data, T predecessor = default, float currentPathLength = 0f,
                float distanceToFinish = float.MaxValue) : base(data, predecessor, currentPathLength)
            {
                // TODO why is this one initialized with different default values than the base class????
                // -> currentPathLength is here 0, in the base class (DijkstraNode) it is float.MaxValue
                // -> The algorithms should not rely on the default values
                DistanceToFinish = distanceToFinish;
            }
        }
        #endregion

        /// <summary>
        /// Get a path between two points in the graph using the A* algorithm.
        /// If no path can be found, an empty list is returned.
        /// </summary>
        public static List<T> AStarSolve<T>(this IReadOnlyGraph<T> graph, T start, T end, ICalculator<T> calculator = null)
            where T : IEquatable<T>
        {
            // validate input
            graph.ThrowOnInvalidInput(start, end);

            return graph.AStarSolveWithInfo(start, end, calculator).Path;
        }
        
        /// <summary>
        /// Get a path between two points in the graph using the A* algorithm.
        /// A list of the visited nodes is also returned. 
        /// If no path can be found, the <see cref="PathFindingResult{T}"/> will be empty, but no members will be null.
        /// <br/><see cref="PathFindingResult{T}"/> will contain the found path, the nodes that were queued to be evaluated
        /// (in <see cref="PathFindingResult{T}.OpenNodes"/>) and the nodes that were finally evaluated
        /// (in <see cref="PathFindingResult{T}.ClosedNodes"/>)
        /// </summary>
        public static PathFindingResult<T> AStarSolveWithInfo<T>(this IReadOnlyGraph<T> g, T start, 
            T end, ICalculator<T> calculator = null)
            where T : IEquatable<T>
        {
            // TODO if start and end are the same, immediately return an empty list
            g.ThrowOnInvalidInput(start, end);

            // check if another calculator has been provided, if not choose the one from the graph
            calculator = calculator ?? g.Calculator;

            // initialize the open and closed list
            // the closed list contains all nodes that we don't want to evaluate ever again, they are done
            var closed = new HashSet<AStarNode<T>>();
            // the open list contains the nodes that are queued up for evaluation at some point
            var open = new HashSet<AStarNode<T>>
                // first we enqueue the provided start node, with an estimated distance of start to end
                // it should be possible to make this float.maxValue instead
                {new AStarNode<T>(start, distanceToFinish: g.Calculator.Distance(start, end))};

            // we have nodes to evaluate
            while (open.Count > 0)
            {
                // get most promising node
                var recordHolder = AStarGetMostPromisingNode(open);

                // we don't want a null record holder something went wrong
                // TODO this will go in an infinite loop if record holder ever is null
                // it probably won't be, but this is bad design!
                if(recordHolder == null) continue;
                
                // check if the record node is the finish and return the corresponding data if it is
                if (recordHolder.Data.Equals(end))
                {
                    return new PathFindingResult<T>(BuildPath(recordHolder, closed),
                        new HashSet<T>(open.Select(x => x.Data)),
                        new HashSet<T>(closed.Select(x => x.Data)));
                }
                
                // if we chose a node, we are kind of done with evaluating it again
                // (there are scenarios where that might be a good idea)
                open.Remove(recordHolder);
                closed.Add(recordHolder);
                
                // expand the record holder.
                AStarExpandNode(recordHolder, end, g.GetPassableConnections(recordHolder.Data), open, closed,
                    calculator);
            }
            // if we leave the loop (we have no nodes to evaluate), we have no path
            return PathFindingResult<T>.Empty;
        }

        /// <summary>
        /// Get the most promising node from the provided open list.
        /// It will search for the node with the lowest F value
        /// (<see cref="AStarNode{T}.EstimatedCompletePathLength"/>)
        /// </summary>
        private static AStarNode<T> AStarGetMostPromisingNode<T>(HashSet<AStarNode<T>> open) where T : IEquatable<T>
        {
            AStarNode<T> recordHolder = null;
            var record = float.MaxValue;
            // TODO try out the LINQ version (looks more readable, debatable if it actually does the same)
            foreach (var node in open)
            {
                if (node.EstimatedCompletePathLength >= record) continue;
                record = node.EstimatedCompletePathLength;
                recordHolder = node;
            }

            return recordHolder;
        }

        /// <summary>
        /// Expand a node, which means add all the neighbors to the open list, initialize them with the correct
        /// values (if that hasn't been done) and update them if necessary (a better path to them is found)
        /// </summary>
        /// <param name="node">The node to expand</param>
        /// <param name="finish">The finish we strive for</param>
        /// <param name="neighbors">The neighbors of the passed node (to prevent passing the graph)</param>
        /// <param name="open">The list of nodes that might be expanded later</param>
        /// <param name="closed">The list of nodes we don't want to expand later</param>
        /// <param name="calculator">The calculator we want to use to calculate distances between nodes</param>
        private static void AStarExpandNode<T>(DijkstraNode<T> node, T finish, IEnumerable<T> neighbors,
            ICollection<AStarNode<T>> open, ICollection<AStarNode<T>> closed, ICalculator<T> calculator) where T : IEquatable<T>
        {
            foreach (var neighbor in neighbors)
            {
                var n = new AStarNode<T>(neighbor);

                // calculate 
                var currentPathLength = node.CurrentPathLength + calculator.Distance(node.Data, neighbor);

                // on the basis of the containing node only (no heuristic, predecessor, etc.)
                var inOpen = open.Contains(n);
                var inClosed = closed.Contains(n);
                
                // TODO we actually never get the values from the list to compare them so this never fires
                if((inOpen || inClosed) && currentPathLength >= n.CurrentPathLength) continue;

                n.Predecessor = node.Data;
                n.CurrentPathLength = currentPathLength;
                n.DistanceToFinish = calculator.Distance(neighbor, finish);

                if (inClosed || !inOpen)
                    open.Add(n);
            }
        }
    }
}