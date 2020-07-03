using System;
using CMGTGraph.Types;

namespace CMGTGraph.Calculators
{
    /// <summary>
    /// A special calculator using Diagonal distance on integer points (<see cref="Point"/>). 
    /// Use this if you can move in 8 directions.
    /// </summary>
    public class PointDiagonalCalculator : ICalculator<Point>
    {
        /// <summary>
        /// The value that is used to weigh straight edges.
        /// </summary>
        private readonly float _straight;

        /// <summary>
        /// The value that is used to weigh diagonal edges.
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
        private PointDiagonalCalculator(float straight = 1f, float diagonal = 1.41421356237f)
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
        public static readonly PointDiagonalCalculator Octile;

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
        public static readonly PointDiagonalCalculator Chebyshev;

        static PointDiagonalCalculator()
        {
            Octile = new PointDiagonalCalculator(1f, (float) Math.Sqrt(2f));
            Chebyshev = new PointDiagonalCalculator(1f, 1f);
        }

        // TODO these functions don't throw errors when something is null
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