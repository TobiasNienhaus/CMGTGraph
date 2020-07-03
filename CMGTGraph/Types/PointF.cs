using System;
using System.Globalization;

namespace CMGTGraph.Types
{
    /// <summary>
    /// A simple class that represents a point in 2D space with floating point values.
    /// </summary>
    public class PointF : IEquatable<PointF>
    {
        // TODO this should REALLY be a struct
        // TODO make them inherit from an interface like IPoint for example
        public readonly float X;
        public readonly float Y;

        /// <summary>
        /// Create a floating point Point. The values are completely readonly and cannot be changed after construction.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public PointF(float x, float y)
        {
            X = x;
            Y = y;
        }

        /// <inheritdoc />
        public bool Equals(PointF other)
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
            return obj.GetType() == GetType() && Equals((PointF) obj);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }

        public static bool operator ==(PointF left, PointF right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(PointF left, PointF right)
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