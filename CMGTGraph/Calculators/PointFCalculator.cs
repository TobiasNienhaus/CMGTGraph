using System;
using System.Linq;
using CMGTGraph.Types;

namespace CMGTGraph.Calculators
{
    public class PointFCalculator : ICalculator<PointF>
    {
        /// <inheritdoc />
        public bool Equals(PointF a, PointF b)
        {
            return a.Equals(b);
        }

        /// <inheritdoc />
        public float Distance(PointF a, PointF b)
        {
            if (Equals(a, b)) return 0f;
            ThrowIfNull(a, b);

            return Length(Add(a, b));
        }

        /// <inheritdoc />
        public float Length(PointF t)
        {
            ThrowIfNull(t);
            return MathF.Sqrt(t.X * t.X + t.Y * t.Y);
        }

        /// <inheritdoc />
        public PointF Subtract(PointF a, PointF b)
        {
            ThrowIfNull(a, b);
            return new PointF(a.X - b.X, a.Y - b.Y);
        }

        /// <inheritdoc />
        public PointF Add(PointF a, PointF b)
        {
            ThrowIfNull(a, b);
            return new PointF(a.X + b.X, a.Y + b.Y);
        }

        private static void ThrowIfNull(PointF a)
        {
            if(a == null) throw new ArgumentNullException(nameof(a));
        }
        
        private static void ThrowIfNull(PointF a, PointF b)
        {
            if(a == null) throw new ArgumentNullException(nameof(a));
            if(b == null) throw new ArgumentNullException(nameof(b));
        }
        
        private static void ThrowIfNull(params PointF[] p)
        {
            if (p.Length == 0) return;
            if (p.Any(point => point == null))
            {
                throw new ArgumentNullException();
            }
        }
    }
}