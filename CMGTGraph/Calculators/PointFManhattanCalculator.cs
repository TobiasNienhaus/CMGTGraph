using System;
using CMGTGraph.Types;

namespace CMGTGraph.Calculators
{
    /// <summary>
    /// 
    /// </summary>
    public class PointFManhattanCalculator : ICalculator<PointF>
    {
        public static readonly PointFManhattanCalculator This;

        private PointFManhattanCalculator()
        {
        }

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