using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using CMGTGraph;
using CMGTGraph.Calculators;
using CMGTGraph.Types;
using NUnit.Framework;

namespace CMGTGraphTest
{
    public class PredefinedTypesTest
    {
        private Random _random;

        [SetUp]
        public void SetUp()
        {
            _random = new Random();
        }
        
        [Test]
        public void PointFTest()
        {
            var p1 = new PointF(0f, 0f);
            var p11 = new PointF(0f, 0f);
            var p2 = new PointF(0f, 1f);
            Assert.IsNotNull(PointFCalculator.This);

            Assert.IsNotNull(p1.ToString());

            // Point.GetHashCode()
            for (var i = 0; i < 10000; i++)
            {
                var x = _random.Next();
                var y = _random.Next();
                var a = new PointF(x, y);
                var b = new PointF(x, y);
                Assert.True(a.GetHashCode() == b.GetHashCode());
            }
            
            Assert.IsTrue(Math.Abs(new PointF(1f, 2f).X - 1f) < float.Epsilon);
            Assert.IsTrue(Math.Abs(new PointF(1f, 2f).Y - 2f) < float.Epsilon);
            
            // PointF.Equals(PointF)
            Assert.IsFalse(p1.Equals(null));
            Assert.IsTrue(p1.Equals(p1));
            Assert.IsTrue(p1.Equals(p11));
            Assert.IsFalse(p1.Equals(p2));
            
            // PointF.Equals(object)
            Assert.IsFalse(p1.Equals((object) null));
            Assert.IsTrue(p1.Equals((object) p1));
            Assert.IsTrue(p1.Equals((object) p11));
            Assert.IsFalse(p1.Equals((object) p2));
            
            // PointF == PointF
            Assert.IsTrue(p1 == p11);
            Assert.IsFalse(p1 == p2);
            // PointF != PointF
            Assert.IsFalse(p1 != p11);
            Assert.IsTrue(p1 != p2);
        }

        [Test]
        public void PointFCalculatorTest()
        {
            var p1 = new PointF(3f, 4f);
            var p2 = new PointF(6f, 8f);
            TestCalculator(PointFCalculator.This, null, p1, p2, 5f, 25f);
        }

        [Test]
        public void PointTest()
        {
            var p1 = new Point(0, 0);
            var p11 = new Point(0, 0);
            var p2 = new Point(0, 1);
            Assert.IsNotNull(PointCalculator.This);

            Assert.IsTrue(new Point(1, 2).X == 1);
            Assert.IsTrue(new Point(1, 2).Y == 2);
            
            Assert.IsNotNull(p1.ToString());
            
            // Point.GetHashCode()
            for (var i = 0; i < 10000; i++)
            {
                var x = _random.Next();
                var y = _random.Next();
                var a = new Point(x, y);
                var b = new Point(x, y);
                Assert.True(a.GetHashCode() == b.GetHashCode());
            }

            // Point.Equals(Point)
            Assert.IsFalse(p1.Equals(null));
            Assert.IsTrue(p1.Equals(p1));
            Assert.IsTrue(p1.Equals(p11));
            Assert.IsFalse(p1.Equals(p2));

            // Point.Equals(object)
            Assert.IsFalse(p1.Equals((object) null));
            Assert.IsTrue(p1.Equals((object) p1));
            Assert.IsTrue(p1.Equals((object) p11));
            Assert.IsFalse(p1.Equals((object) p2));

            // Point == Point
            Assert.IsTrue(p1 == p11);
            Assert.IsFalse(p1 == p2);
            
            // Point != Point
            Assert.IsFalse(p1 != p11);
            Assert.IsTrue(p1 != p2);
        }

        [Test]
        public void PointCalculatorTest()
        {
            var p1 = new Point(3, 4);
            var p2 = new Point(6, 8);
            TestCalculator(PointCalculator.This, null, p1, p2, 5f, 25f);
        }

        [Test]
        public void MiscCalculatorsTest()
        {
            var p1 = new Point(3, 4);
            var p2 = new Point(6, 8);
            var pf1 = new PointF(3, 4);
            var pf2 = new PointF(6, 8);
            
            TestCalculator(PointManhattanCalculator.This, null, p1, p2, 7, 49);
            TestCalculator(PointFManhattanCalculator.This, null, pf1, pf2, 7, 49);

            const float dist = 5.242640687f;
            const float distSqr = dist * dist;
            TestCalculator(PointDiagonalCalculator.Octile, null, p1, p2, dist, distSqr, 0.00001f);
            TestCalculator(PointFDiagonalCalculator.Octile, null, pf1, pf2, dist, distSqr, 0.00001f);
            
            TestCalculator(PointDiagonalCalculator.Chebyshev, null, p1, p2, 4f, 16f);
            TestCalculator(PointFDiagonalCalculator.Chebyshev, null, pf1, pf2, 4f, 16f);
        }

        private static void TestCalculator<T>(ICalculator<T> c, T @null, T a, T b, float distAb, float sqrDistAb, float accuracy = float.Epsilon) where T : IEquatable<T>
        {
            Assert.IsTrue(Math.Abs(c.Distance(a, b) - distAb) < accuracy);
            Assert.Catch(() => c.Distance(a, @null));
            Assert.Catch(() => c.Distance(@null, a));
            
            Assert.IsTrue(Math.Abs(c.SqrDistance(a, b) - sqrDistAb) < accuracy);
            Assert.Catch(() => c.SqrDistance(a, @null));
            Assert.Catch(() => c.SqrDistance(@null, a));
        }
    }
}