using System;
using CMGTGraph.Types;

namespace CMGTGraph.Calculators
{
    public class PointCalculator : ICalculator<Point>
    {
        private PointCalculator(){}

        public static readonly PointCalculator This;

        static PointCalculator() => This = new PointCalculator();
        
        /// <inheritdoc />
        public float SqrDistance(Point a, Point b)
        {
            ThrowIfNull(a, b);
            var x = b.X - a.X;
            var y = b.Y - a.Y;
            return x * x + y * y;
        }

        /// <inheritdoc />
        public float Distance(Point a, Point b)
        {
            return (float) Math.Sqrt(SqrDistance(a, b));
        }

        private static void ThrowIfNull(Point a, Point b)
        {
            if (a == null) throw new ArgumentNullException(nameof(a));
            if (b == null) throw new ArgumentNullException(nameof(b));
        }
    }
}