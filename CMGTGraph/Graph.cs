using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using CMGTGraph.Calculators;

namespace CMGTGraph
{
    // TODO add IReadOnlyGraph interface to make it possible to provide a immutable copy of the graph
    
    /// <summary>
    /// The representation of a graph.
    /// </summary>
    /// <typeparam name="T">T needs to have a robust GetHashCode() implementation, as many operations rely on it in here</typeparam>
    public class Graph<T> : IReadOnlyGraph<T> where T : IEquatable<T>
    {
        public sealed class NodeNotFoundException : KeyNotFoundException
        {
            public NodeNotFoundException()
            { }
            public NodeNotFoundException(T value)
            {
                Data.Add("ToString", value.Equals(null) ? value.ToString() : "null");
            }
        }
        
        private readonly Dictionary<T, HashSet<T>> _connections;

        /// <inheritdoc />
        public int NodeCount => _connections.Count;

        /// <inheritdoc />
        public ICalculator<T> Calculator { get; }

        private readonly HashSet<T> _impassable;
        
        /// <summary>
        /// Create a new graph.
        /// </summary>
        /// <param name="calculator">The calculator that can be used to calculate useful things for the specified type</param>
        public Graph(ICalculator<T> calculator)
        {
            Calculator = calculator;
            _connections = new Dictionary<T, HashSet<T>>();
            _impassable = new HashSet<T>();
        }
        
        /// <summary>
        /// Add a node to the graph. If the node is already in the graph, nothing happens.
        /// </summary>
        public void Add(T node)
        {
            if(!_connections.ContainsKey(node))
                _connections.Add(node, new HashSet<T>());
        }
        
        /// <summary>
        /// Add a connection between two nodes. If one or both of the nodes don't/doesn't
        /// exist in the graph, it/they will be added to the graph. 
        /// </summary>
        public void AddConnection(T nodeA, T nodeB)
        {
            Add(nodeA);
            Add(nodeB);
            _connections[nodeA].Add(nodeB);
            _connections[nodeB].Add(nodeA);
        }
        
        /// <summary>
        /// Remove the provided node and all connections to that node.
        /// </summary>
        /// <returns>Returns true, if the node could be removed successfully.</returns>
        public bool RemoveNode(T node)
        {
            if (!_connections.Remove(node)) return false;
            foreach (var connection in _connections)
            {
                connection.Value.Remove(node);
            }

            return true;
        }

        /// <summary>
        /// Remove the connection between the two nodes.
        /// </summary>
        /// <returns>Returns false, if one of the nodes isn't in the graph or if no connection has been removed.</returns>
        public bool RemoveConnection(T nodeA, T nodeB)
        {
            if (!_connections.ContainsKey(nodeA) || !_connections.ContainsKey(nodeB)) return false;
            
            return _connections[nodeA].Remove(nodeB) | _connections[nodeB].Remove(nodeA);
        }
        
        /// <summary>
        /// Get the connections of a node in a graph.
        /// </summary>
        /// <exception cref="NodeNotFoundException"></exception>
        public HashSet<T> GetConnections(T node)
        {
            // TODO the returned HashSet can still be altered
            try
            {
                return _connections[node];
            }
            catch (KeyNotFoundException)
            {
                throw new NodeNotFoundException();
            }
        }

        /// <inheritdoc />
        public HashSet<T> GetPassableConnections(T node)
        {
            var conn = new HashSet<T>(GetConnections(node));
            conn.ExceptWith(_impassable);
            return conn;
        }

        /// <summary>
        /// Test if the provided nodes have a connection in the graph. Returns true if yes.
        /// </summary>
        /// <exception cref="NodeNotFoundException">Thrown when one of the provided nodes is not in the graph</exception>
        public bool HaveConnection(T nodeA, T nodeB)
        {
            if(!Contains(nodeA)) throw new NodeNotFoundException(nodeA);
            if(!Contains(nodeB)) throw new NodeNotFoundException(nodeB);

            return _connections[nodeA].Contains(nodeB) || _connections[nodeB].Contains(nodeA);
        }
        
        /// <summary>
        /// Delete all nodes and connections from the graph.
        /// </summary>
        public void Clear()
        {
            foreach (var connection in _connections)
            {
                connection.Value.Clear();
            }
            _connections.Clear();
        }

        /// <inheritdoc />
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine("---------------------");
            sb.AppendLine($"Graph ({NodeCount.ToString()} nodes)");
            foreach (var t in _connections)
            {
                sb.AppendLine($"Node: {t.Key.ToString()}");
                foreach (var x1 in t.Value)
                {
                    sb.AppendLine($"\tConnection to: {x1.ToString()}");
                }
            }
            sb.AppendLine("---------------------");
            return sb.ToString();
        }

        public bool Contains(T value)
        {
            return _connections.ContainsKey(value);
        }

        /// <inheritdoc />
        public bool NodeIsPassable(T node)
        {
            return !_impassable.Contains(node);
        }

        public void MakeImpassable(T node)
        {
            if (!_impassable.Contains(node)) _impassable.Add(node);
        }

        public void MakePassable(T node)
        {
            if (_impassable.Contains(node)) _impassable.Remove(node);
        }

        public void TogglePassable(T node)
        {
            if (_impassable.Contains(node)) _impassable.Remove(node);
            else _impassable.Add(node);
        }

        /// <inheritdoc />
        public bool IsConnected()
        {
            var startNode = _connections.Keys.First();
            var visited = new HashSet<T> {startNode};
            var q = new Queue<T>();
            q.Enqueue(startNode);
            
            while (q.Count > 0)
            {
                var n = q.Dequeue();
                foreach (var nb in GetPassableConnections(n))
                {
                    if(visited.Contains(nb)) continue;
                    q.Enqueue(nb);
                    visited.Add(nb);
                }
            }

            if (visited.Count > NodeCount)
                throw new Exception("Somehow more nodes where visited then are in the graph.");
            return visited.Count == NodeCount;
        }
    }
}