using System;
using CMGTGraph.Types;

namespace CMGTGraph.Calculators
{
    /// <inheritdoc/>
    /// The type is in this case <see cref="PointF"/>
    /// <br/>The distance is calculated using manhattan distance, which is basically
    /// the distance on both axis added together.
    /// <br/>Use this, if you can move in 4 directions (on a grid).
    public class PointFManhattanCalculator : ICalculator<PointF>
    {
        /// <summary>
        /// The instance of this calculator.
        /// </summary>
        public static readonly PointFManhattanCalculator This;

        /// <summary>
        /// Don't construct your own, use <see cref="This"/>!
        /// </summary>
        private PointFManhattanCalculator()
        { }

        static PointFManhattanCalculator() => This = new PointFManhattanCalculator();

        /// <inheritdoc />
        public float SqrDistance(PointF a, PointF b)
        {
            var d = Distance(a, b);
            return d * d;
        }

        /// <inheritdoc />
        public float Distance(PointF a, PointF b)
        {
            return Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);
        }
    }
}