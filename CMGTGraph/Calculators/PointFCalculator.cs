using System;
using System.Linq;
using CMGTGraph.Types;

namespace CMGTGraph.Calculators
{
    /// <inheritdoc/>
    /// T is in this case of type <see cref="PointF"/>.
    /// The distance calculation used here is Euclidean distance.
    public class PointFCalculator : ICalculator<PointF>
    {
        private PointFCalculator()
        { }

        /// <summary>
        /// The instance of this calculator.
        /// </summary>
        public static readonly PointFCalculator This;

        static PointFCalculator() => This = new PointFCalculator();
        
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

        /// <summary>
        /// Throw an <see cref="ArgumentNullException"/> if one of the provided arguments is null.
        /// </summary>
        private static void ThrowIfNull(PointF a, PointF b)
        {
            if(a == null) throw new ArgumentNullException(nameof(a));
            if(b == null) throw new ArgumentNullException(nameof(b));
        }
    }
}