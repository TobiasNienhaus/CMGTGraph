﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMGTGraph.Calculators;

namespace CMGTGraph
{
	// TODO look into ALT A* (https://www.microsoft.com/en-us/research/wp-content/uploads/2004/07/tr-2004-24.pdf)

    /// <summary>
    /// The representation of a graph.
    /// </summary>
    /// <typeparam name="T">T needs to have a robust GetHashCode() implementation, as many operations rely on it in here</typeparam>
    public class Graph<T> : IReadOnlyGraph<T> where T : IEquatable<T>
    {
        /// <summary>
        /// An exception that can be thrown when a specific node is not found is not found in the graph.
        /// <br/>It contains the node that was requested to be found in string represantation.
        /// </summary>
        public sealed class NodeNotFoundException : KeyNotFoundException
        {
            /// <summary>
            /// Create a new <see cref="NodeNotFoundException"/> with the value that was tried to be found,
            /// but was not found. This node will be stored in the exception data in string representation.
            /// </summary>
            /// <param name="value"></param>
            public NodeNotFoundException(T value)
            {
                Data.Add("ToString", value.Equals(null) ? value.ToString() : "null");
            }
        }
        
        /// <summary>
        /// The underlying representation of the graph as an adjacency list.
        /// </summary>
        private readonly Dictionary<T, HashSet<T>> _connections;

        /// <inheritdoc />
        public int NodeCount => _connections.Count;

        /// <inheritdoc />
        public HashSet<T> Nodes => new HashSet<T>(_connections.Keys);

        // TODO maybe make the calculator not a member of graph, but a thing that is e.g. passed to functions
        /// <inheritdoc />
        public ICalculator<T> Calculator { get; set; }

        /// <summary>
        /// The collection of nodes that is not passable.
        /// </summary>
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
        /// If you pass the same node twice, it will be added to the graph but no connections will be added
        /// as to not create loops. 
        /// </summary>
        public void AddConnection(T nodeA, T nodeB)
        {
            Add(nodeA);
            if (nodeA.Equals(nodeB)) return;
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

        /// <inheritdoc />
        public HashSet<T> GetPassableConnections(T node)
        {
            if(!Contains(node)) throw new NodeNotFoundException(node);
            var conn = new HashSet<T>(_connections[node]);
            conn.ExceptWith(_impassable);
            return conn;
        }

        /// <summary>
        /// Test if the provided nodes have a connection in the graph. Returns true if yes.
        /// </summary>
        /// <exception cref="NodeNotFoundException">Thrown when one of the provided nodes is not in the graph</exception>
        public bool HaveConnection(T nodeA, T nodeB)
        {
            // TODO maybe don't throw exception, but return false
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

        /// <summary>
        /// Is a specific node in the Graph?
        /// </summary>
        /// <param name="value">The node to check</param>
        /// <returns>Returns true, if the passed node is in the graph</returns>
        public bool Contains(T value)
        {
            return _connections.ContainsKey(value);
        }

        /// <inheritdoc />
        public bool NodeIsPassable(T node)
        {
            return !_impassable.Contains(node);
        }

        /// <summary>
        /// Make a node impassable. Has no effect, if the node is already impassable.
        /// </summary>
        public void MakeImpassable(T node)
        {
            if (!_impassable.Contains(node)) _impassable.Add(node);
        }

        /// <summary>
        /// Make a node passable. Has no effect, if the node is already passable.
        /// </summary>
        public void MakePassable(T node)
        {
            if (_impassable.Contains(node)) _impassable.Remove(node);
        }

        /// <summary>
        /// Toggle, if a node is passable or not, i.e. make it passable when it's not and make it impassable when it is.
        /// </summary>
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