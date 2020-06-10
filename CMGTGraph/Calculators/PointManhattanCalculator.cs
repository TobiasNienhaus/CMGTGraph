using System;
using CMGTGraph.Types;

namespace CMGTGraph.Calculators
{
    /// <summary>
    /// This uses Manhattan square distance as the distance calculation.
    /// Use this when you can move in 4 directions
    /// </summary>
    public class PointManhattanCalculator : ICalculator<Point>
    {
        public static readonly PointManhattanCalculator This;

        private PointManhattanCalculator()
        { }

        static PointManhattanCalculator() => This = new PointManhattanCalculator();

        /// <inheritdoc />
        public float SqrDistance(Point a, Point b)
        {
            var d = Distance(a, b);
            return d * d;
        }

        /// <inheritdoc />
        public float Distance(Point a, Point b)
        {
            return Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);
        }
    }
}