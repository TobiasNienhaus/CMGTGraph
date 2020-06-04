using System;
using System.Collections.Generic;
using System.Linq;
using CMGTGraph.Logging;

namespace CMGTGraph.Algorithms
{
    public static class Algorithms
    {
        #region Node
        public class Node<T> : IEquatable<Node<T>> where T : IEquatable<T>
        {
            public readonly T Data;
            public T Predecessor;
            public float CurrentPathLength;
            public float EstimatedCompletePathLength;

            public Node(T data, T predecessor = default, float currentPathLength = float.MaxValue,
                float estimatedCompletePathLength = float.MinValue)
            {
                Data = data;
                Predecessor = predecessor;
                CurrentPathLength = currentPathLength;
                EstimatedCompletePathLength = estimatedCompletePathLength;
            }

            /// <inheritdoc />
            public bool Equals(Node<T> other)
            {
                if (ReferenceEquals(null, other)) return false;
                return ReferenceEquals(this, other) || Data.Equals(other.Data);
            }

            /// <inheritdoc />
            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                return obj.GetType() == GetType() && Equals((Node<T>) obj);
            }

            /// <inheritdoc />
            public override int GetHashCode()
            {
                return Data.GetHashCode();
            }

            public static bool operator ==(Node<T> left, Node<T> right)
            {
                return Equals(left, right);
            }

            public static bool operator !=(Node<T> left, Node<T> right)
            {
                return !Equals(left, right);
            }
        }
        #endregion

        public static List<T> AStarSolve<T>(this Graph<T> graph, T start, T end)
            where T : IEquatable<T>
        {
            if (!graph.Contains(start))
            {
                Logger.Warn("Start node doesn't exist in graph!");
                throw new Graph<T>.NodeNotFoundException(start);
            }

            if (!graph.Contains(end))
            {
                Logger.Warn("End node doesn't exist in graph!");
                throw new Graph<T>.NodeNotFoundException(end);
            }
            
            // var closed = new Dictionary<T, Node<T>>();
            var closed = new HashSet<Node<T>>();
            // var open = new Dictionary<T, Node<T>>();
            var open = new List<Node<T>> {new Node<T>(start)};

            var connections = graph.Connections;

            do
            {
                var recordHolder = open[0];
                var record = recordHolder.EstimatedCompletePathLength;
                foreach (var node in open)
                {
                    if (!(node.EstimatedCompletePathLength < record)) continue;
                    record = node.EstimatedCompletePathLength;
                    recordHolder = node;
                }

                open.Remove(recordHolder);

                if (recordHolder.Data.Equals(end))
                {
                    var knownNodes = closed.ToDictionary(node => node.Data);
                    var path = BuildPath(recordHolder, knownNodes);
                    path.Reverse();
                    return path;
                }

                closed.Add(recordHolder);
                ExpandNode(recordHolder, end, connections[recordHolder.Data], open, closed, graph.Calculator);
            } while (open.Count > 0);

            return new List<T>();
        }

        // ReSharper disable once ParameterTypeCanBeEnumerable.Local
        private static void ExpandNode<T>(Node<T> node, T finish, HashSet<T> neighbors, List<Node<T>> open,
            ICollection<Node<T>> closed, ICalculator<T> calculator) where T : IEquatable<T>
        {
            foreach (var neighbor in neighbors)
            {
                if (closed.Contains(new Node<T>(neighbor))) continue;

                var newCurrentLength = node.CurrentPathLength + calculator.Distance(node.Data, neighbor);
                
                Node<T> fromOpenList = null;
                foreach (var n in open)
                {
                    if (n.Data.Equals(neighbor)) fromOpenList = n;
                }
                
                if (fromOpenList != null && newCurrentLength >= fromOpenList.CurrentPathLength) continue; // find a way to save Path lengths to nodes
                
                fromOpenList = fromOpenList ?? new Node<T>(neighbor);
                fromOpenList.Predecessor = node.Data;
                fromOpenList.CurrentPathLength = newCurrentLength;

                fromOpenList.EstimatedCompletePathLength = newCurrentLength + calculator.Distance(neighbor, finish);
                
                if (!open.Contains(fromOpenList)) open.Add(fromOpenList);
            }
        }

        private static List<T> BuildPath<T>(Node<T> from, IReadOnlyDictionary<T, Node<T>> knownNodes) where T : IEquatable<T>
        {
            var ret = new List<T>();
            var current = from;
            while (current != null)
            {
                ret.Add(current.Data);

                current = current.Predecessor != null ? knownNodes[current.Predecessor] : null;
            }

            return ret;
        }
    }
}