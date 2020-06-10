using System;
using CMGTGraph.Types;

namespace CMGTGraph.Calculators
{
    /// <summary>
    /// This uses Diagonal distance. Use this if you can move in 8 directions.
    /// </summary>
    public class PointDiagonalCalculator : ICalculator<Point>
    {
        private readonly float _straight;

        private readonly float _diagonal;

        private PointDiagonalCalculator(float straight = 1f, float diagonal = 1.41421356237f)
        {
            _straight = straight;
            _diagonal = diagonal;
        }

        public static readonly PointDiagonalCalculator Octile;

        /// <summary>
        /// Diagonal calculator which treats diagonal and straight edges equally in the heuristic.
        /// </summary>
        public static readonly PointDiagonalCalculator Chebyshev;

        static PointDiagonalCalculator()
        {
            Octile = new PointDiagonalCalculator(1f, (float) Math.Sqrt(2f));
            Chebyshev = new PointDiagonalCalculator(1f, 1f);
        }

        /// <inheritdoc />
        public float SqrDistance(Point a, Point b)
        {
            var d = Distance(a, b);
            return d * d;
        }

        /// <inheritdoc />
        public float Distance(Point a, Point b)
        {
            var dx = Math.Abs(a.X - b.X);
            var dy = Math.Abs(a.Y - b.Y);
            return _straight * (dx + dy) + (_diagonal - 2 * _straight) * Math.Min(dx, dy);
        }
    }
}