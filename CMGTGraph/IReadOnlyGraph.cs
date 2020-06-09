using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace CMGTGraph
{
    public interface IReadOnlyGraph<T> where T : IEquatable<T>
    {
        // TODO connections can still be altered through this
        ReadOnlyDictionary<T, HashSet<T>> Connections { get; }
        
        int NodeCount { get; }
        
        ICalculator<T> Calculator { get; }

        HashSet<T> GetConnections(T node);

        bool HaveConnection(T nodeA, T nodeB);

        bool Contains(T value);
    }
}