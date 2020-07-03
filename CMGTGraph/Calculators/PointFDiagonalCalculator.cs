using System;
using CMGTGraph.Types;

namespace CMGTGraph.Calculators
{
    /// <summary>
    /// A special calculator using Diagonal distance on integer points (<see cref="Point"/>). 
    /// Use this if you can move in 8 directions.
    /// </summary>
    public class PointFDiagonalCalculator : ICalculator<PointF>
    {
        /// <summary>
        /// The weight for straight edges.
        /// </summary>
        private readonly float _straight;
        /// <summary>
        /// The weight for diagonal edges.
        /// </summary>
        private readonly float _diagonal;

        /// <summary>
        /// <para>
        /// Create a new calculator with new weights for straight and diagonal edges.
        /// The default values are 1 for straight edges and SQRT(2) for diagonal ones.
        /// </para>
        /// <para>
        /// This implicitly hides the constructor with default values,
        /// to prevent creating a new instance on the outside.
        /// </para>
        /// </summary>
        private PointFDiagonalCalculator(float straight = 1f, float diagonal = 1.41421356237f)
        {
            _straight = straight;
            _diagonal = diagonal;
        }

        /// <summary>
        /// <para>
        /// A calculator using octile diagonal distance.
        /// </para>
        /// <para>
        /// Read more on grid heuristics here:
        /// http://theory.stanford.edu/~amitp/GameProgramming/Heuristics.html#heuristics-for-grid-maps
        /// </para>
        /// </summary>
        public static readonly PointFDiagonalCalculator Octile;
        /// <summary>
        /// <para>
        /// A calculator using chebyshev diagonal distance,
        /// which basically means diagonal and straight edges have the same weight,
        /// thus diagonal edges are preferred using this calculator as a heuristic, compared to using 
        /// octile distance (with <see cref="Octile"/>).
        /// </para>
        /// <para>
        /// Read more on grid heuristics here:
        /// http://theory.stanford.edu/~amitp/GameProgramming/Heuristics.html#heuristics-for-grid-maps
        /// </para>
        /// </summary>
        public static readonly PointFDiagonalCalculator Chebyshev;

        static PointFDiagonalCalculator()
        {
            Octile = new PointFDiagonalCalculator();
            Chebyshev = new PointFDiagonalCalculator(1f, 1f);
        }
        
        // TODO these functions don't throw errors when the arguments are null
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