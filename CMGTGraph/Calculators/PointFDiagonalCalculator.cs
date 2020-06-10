using System;
using System.Runtime.CompilerServices;
using CMGTGraph.Types;

namespace CMGTGraph.Calculators
{
    public class PointFDiagonalCalculator : ICalculator<PointF>
    {
        private readonly float _straight;
        // SQRT 2
        private readonly float _diagonal;

        private PointFDiagonalCalculator(float straight = 1f, float diagonal = 1.41421356237f)
        {
            _straight = straight;
            _diagonal = diagonal;
        }

        public static readonly PointFDiagonalCalculator Octile;
        /// <summary>
        /// Diagonal calculator which treats diagonal and straight edges equally in the heuristic.
        /// </summary>
        public static readonly PointFDiagonalCalculator Chebyshev;

        static PointFDiagonalCalculator()
        {
            Octile = new PointFDiagonalCalculator();
            Chebyshev = new PointFDiagonalCalculator(1f, 1f);
        }
        
        /// <inheritdoc />
        public float SqrDistance(PointF a, PointF b)
        {
            var d = Distance(a, b);
            return d * d;
        }

        /// <inheritdoc />
        public float Distance(PointF a, PointF b)
        {
            var dx = Math.Abs(a.X - b.X);
            var dy = Math.Abs(a.Y - b.Y);
            return _straight * (dx + dy) + (_diagonal - 2 * _straight) * Math.Min(dx, dy);
        }
    }
}