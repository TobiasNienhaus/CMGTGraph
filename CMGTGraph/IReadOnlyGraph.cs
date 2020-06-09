﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using CMGTGraph.Calculators;

namespace CMGTGraph
{
    public interface IReadOnlyGraph<T> where T : IEquatable<T>
    {
        /// <summary>
        /// The number of nodes in the graph.
        /// </summary>
        int NodeCount { get; }
        
        /// <summary>
        /// The calculator that can be used to calculate things like SqrDistance between nodes.
        /// </summary>
        ICalculator<T> Calculator { get; }
        
        /// <summary>
        /// Get all the nodes that are connected to this node, provided they are passable.
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        HashSet<T> GetPassableConnections(T node);

        /// <summary>
        /// Check if two nodes are connected
        /// </summary>
        bool HaveConnection(T nodeA, T nodeB);

        /// <summary>
        /// Check if a node is in the graph.
        /// </summary>
        bool Contains(T value);
        
        /// <summary>
        /// Check if a node is passable.
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        bool NodeIsPassable(T node);
        
        /// <summary>
        /// Check if the graph is connected (every node has a path to every other node). This takes into account, 
        /// if the node is passable.
        /// </summary>
        /// <returns></returns>
        bool IsConnected();
    }
}