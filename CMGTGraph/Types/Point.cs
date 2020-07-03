using System;
using System.Globalization;

namespace CMGTGraph.Types
{
    /// <summary>
    /// A simple class that represents a point in 2D space using integers.
    /// </summary>
    public class Point : IEquatable<Point>
    {
        // TODO this should be a struct
        // TODO both Point and PointF should inherit from a struct because then calculators can become 1
        public readonly int X;
        public readonly int Y;

        /// <summary>
        /// Create a new integer Point. All the values are readonly.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        /// <inheritdoc />
        public bool Equals(Point other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return X.Equals(other.X) && Y.Equals(other.Y);
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((Point) obj);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }

        public static bool operator ==(Point left, Point right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Point left, Point right)
        {
            return !Equals(left, right);
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return
                $"{nameof(X)}: {X.ToString(CultureInfo.InvariantCulture)}; {nameof(Y)}: {Y.ToString(CultureInfo.InvariantCulture)}";
        }
    }
}