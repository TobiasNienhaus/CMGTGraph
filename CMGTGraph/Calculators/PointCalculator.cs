using System;
using CMGTGraph.Types;

namespace CMGTGraph.Calculators
{
    public class PointCalculator : ICalculator<Point>
    {
        /// <inheritdoc />
        public bool Equals(Point a, Point b)
        {
            return a.Equals(b);
        }

        /// <inheritdoc />
        public float Distance(Point a, Point b)
        {
            ThrowIfNull(a, b);
            return Length(Subtract(b, a));
        }

        /// <inheritdoc />
        public float Length(Point t)
        {
            ThrowIfNull(t);
            return (float) Math.Sqrt(t.X * t.X + t.Y * t.Y);
        }

        /// <inheritdoc />
        public Point Subtract(Point a, Point b)
        {
            ThrowIfNull(a, b);
            return new Point(a.X - b.X, a.Y - b.Y);
        }

        /// <inheritdoc />
        public Point Add(Point a, Point b)
        {
            ThrowIfNull(a, b);
            return new Point(a.X + b.X, a.Y + b.Y);
        }
        
        // TODO add ThrowIfNull functions and use them

        private static void ThrowIfNull(Point a)
        {
            if (a == null) throw new ArgumentNullException(nameof(a));
        }

        private static void ThrowIfNull(Point a, Point b)
        {
            if (a == null) throw new ArgumentNullException(nameof(a));
            if (b == null) throw new ArgumentNullException(nameof(b));
        }
    }
}