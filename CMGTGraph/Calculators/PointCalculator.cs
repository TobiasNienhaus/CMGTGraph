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
            return Length(Subtract(b, a));
        }

        /// <inheritdoc />
        public float Length(Point t)
        {
            return MathF.Sqrt(t.X * t.X + t.Y * t.Y);
        }

        /// <inheritdoc />
        public Point Subtract(Point a, Point b)
        {
            return new Point(a.X - b.X, a.Y - b.Y);
        }

        /// <inheritdoc />
        public Point Add(Point a, Point b)
        {
            return new Point(a.X + b.X, a.Y + b.Y);
        }
    }
}