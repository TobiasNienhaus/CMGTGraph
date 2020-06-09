﻿using System;
using System.Collections.Generic;
using System.Linq;
using CMGTGraph.Logging;

namespace CMGTGraph.Algorithms
{
    public static partial class Algorithms
    {
        public class Node<T> : IEquatable<Node<T>> where T : IEquatable<T>
        {
            public readonly T Data;
            public T Predecessor;

            public Node(T data, T predecessor = default)
            {
                Data = data;
                Predecessor = predecessor;
            }

            #region IEquatable

            /// <inheritdoc />
            public bool Equals(Node<T> other)
            {
                if (ReferenceEquals(null, other)) return false;
                if (ReferenceEquals(this, other)) return true;
                return Data.Equals(other.Data);
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
                return EqualityComparer<T>.Default.GetHashCode(Data);
            }

            public static bool operator ==(Node<T> left, Node<T> right)
            {
                return Equals(left, right);
            }

            public static bool operator !=(Node<T> left, Node<T> right)
            {
                return !Equals(left, right);
            }

            #endregion
        }

        private static void ThrowOnInvalidInput<T>(this IReadOnlyGraph<T> g, T start, T end) where T : IEquatable<T>
        {
            if (!g.Contains(start))
            {
                Logger.Error("Start node doesn't exist in graph!");
                throw new Graph<T>.NodeNotFoundException(start);
            }

            if (!g.Contains(end))
            {
                Logger.Error("End node doesn't exist in graph!");
                throw new Graph<T>.NodeNotFoundException(end);
            }

            if (!g.NodeIsPassable(start))
            {
                Logger.Error("Can't calculate path starting from impassable node.");
                throw new InvalidOperationException();
            }

            if (!g.NodeIsPassable(end))
            {
                Logger.Error("Can't calculate path ending in impassable node.");
                throw new InvalidOperationException();
            }
        }
        
        private static List<T> BuildPath<T>(Node<T> from, IEnumerable<Node<T>> knownNodes) where T : IEquatable<T>
        {
            var p = BuildRecursiveReversePath(from, knownNodes.ToDictionary(n => n.Data));
            p.Reverse();
            return p;
        }

        private static List<T> BuildRecursiveReversePath<T>(Node<T> from, IReadOnlyDictionary<T, Node<T>> knownNodes)
            where T : IEquatable<T>
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