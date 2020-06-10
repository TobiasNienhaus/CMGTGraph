using System;
using System.Linq;
using CMGTGraph.Types;

namespace CMGTGraph.Calculators
{
    public class PointFCalculator : ICalculator<PointF>
    {
        /// <inheritdoc />
        public float SqrDistance(PointF a, PointF b)
        {
            ThrowIfNull(a, b);
            var x = b.X - a.X;
            var y = b.Y - a.Y;
            return x * x + y * y;
        }

        /// <inheritdoc />
        public float Distance(PointF a, PointF b)
        {
            return (float) Math.Sqrt(SqrDistance(a, b));
        }

        private static void ThrowIfNull(PointF a, PointF b)
        {
            if(a == null) throw new ArgumentNullException(nameof(a));
            if(b == null) throw new ArgumentNullException(nameof(b));
        }
    }
}