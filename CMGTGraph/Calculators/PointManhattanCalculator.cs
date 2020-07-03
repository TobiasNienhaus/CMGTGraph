using System;
using CMGTGraph.Types;

namespace CMGTGraph.Calculators
{
    /// <inheritdoc/>
    /// The type is in this case <see cref="Point"/>
    /// <br/>The distance is calculated using manhattan distance, which is basically
    /// the distance on both axis added together.
    /// <br/>Use this, if you can move in 4 directions (on a grid).
    public class PointManhattanCalculator : ICalculator<Point>
    {
        /// <summary>
        /// The instance of this calculator.
        /// </summary>
        public static readonly PointManhattanCalculator This;

        /// <summary>
        /// Don't construct your own, use <see cref="This"/>!
        /// </summary>
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