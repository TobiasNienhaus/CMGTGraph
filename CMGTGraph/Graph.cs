using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using CMGTGraph.Types;

namespace CMGTGraph
{
    /// <summary>
    /// The representation of a graph.
    /// </summary>
    /// <typeparam name="T">T needs to have a robust GetHashCode() implementation, as many operations rely on it in here</typeparam>
    public class Graph<T> where T : IEquatable<T>
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
        
        public ReadOnlyDictionary<T, HashSet<T>> Connections => new ReadOnlyDictionary<T, HashSet<T>>(_connections);
        
        public int NodeCount => _connections.Count;
        
        public ICalculator<T> Calculator { get; }
        
        /// <summary>
        /// Create a new graph.
        /// </summary>
        /// <param name="calculator">The calculator that can be used to calculate useful things for the specified type</param>
        public Graph(ICalculator<T> calculator)
        {
            Calculator = calculator;
            _connections = new Dictionary<T, HashSet<T>>();
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
            try
            {
                return _connections[node];
            }
            catch (KeyNotFoundException e)
            {
                throw new NodeNotFoundException();
            }
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
    }
}