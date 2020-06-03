using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace CMGTGraph
{
    /// <summary>
    /// The representation of a graph.
    /// </summary>
    /// <typeparam name="T">T needs to have a robust GetHashCode() implementation, as many operations rely on it in here</typeparam>
    public class Graph<T> where T : IEquatable<T>
    {
        // use Dictionary<T, List<T>>
        // if you add an edge between nodes
        // then you can do for a room
        // addNode(room center)
        // foreach door -> addEdge(room center, door)
        // 
        // addEdge will add the nodes if necessary and will always add the connection
        // 
        // a path finding algorithm will then return a list of edges
        
        private readonly Dictionary<T, HashSet<T>> _connections;
        
        public ReadOnlyDictionary<T, HashSet<T>> Connections => new ReadOnlyDictionary<T, HashSet<T>>(_connections);
        
        public int NodeCount => _connections.Count;
        
        public ICalculator<T> Calculator { get; }

        public Graph(ICalculator<T> calculator)
        {
            Calculator = calculator;
            _connections = new Dictionary<T, HashSet<T>>();
        }
        
        public void Add(T node)
        {
            if(!_connections.ContainsKey(node))
                _connections.Add(node, new HashSet<T>());
        }
        
        public void AddConnection(T nodeA, T nodeB)
        {
            Add(nodeA);
            Add(nodeB);
            _connections[nodeA].Add(nodeB);
            _connections[nodeB].Add(nodeA);
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