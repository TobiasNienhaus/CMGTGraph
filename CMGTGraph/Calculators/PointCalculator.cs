using System;
using CMGTGraph.Types;

namespace CMGTGraph.Calculators
{
    /// <summary>
    /// A normal calculator to calculate the distance between to integer Points (<see cref="Point"/>)
    /// </summary>
    public class PointCalculator : ICalculator<Point>
    {
        /// <summary>
        /// Hidden so you can't construct it on your own. Also does nothing.
        /// </summary>
        private PointCalculator(){}

        /// <summary>
        /// The static instance of this calculator that should always be null.
        /// </summary>
        public static readonly PointCalculator This;

        static PointCalculator() => This = new PointCalculator();
        
        /// <inheritdoc />
        /// <exception cref="NullReferenceException">if one of the arguments is null</exception>
        public float SqrDistance(Point a, Point b)
        {
            ThrowIfNull(a, b);
            var x = b.X - a.X;
            var y = b.Y - a.Y;
            return x * x + y * y;
        }

        /// <inheritdoc />
        /// <exception cref="NullReferenceException">if one of the arguments is null</exception>
        public float Distance(Point a, Point b)
        {
            return (float) Math.Sqrt(SqrDistance(a, b));
        }

        /// <summary>
        /// Throw <see cref="ArgumentNullException"/> if one of the arguments is null.
        /// </summary>
        private static void ThrowIfNull(Point a, Point b)
        {
            if (a == null) throw new ArgumentNullException(nameof(a));
            if (b == null) throw new ArgumentNullException(nameof(b));
        }
    }
}