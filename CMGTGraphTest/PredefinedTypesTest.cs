using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using CMGTGraph;
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
            Assert.IsNotNull(PointF.Calculator);

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
            var p11 = new PointF(3f, 4f);
            var p2 = new PointF(6f, 8f);
            TestCalculator(PointF.Calculator, null, p1, p11, p2,
                new PointF(9, 12), new PointF(-3, -4), 5f, 5f);
        }

        [Test]
        public void PointTest()
        {
            var p1 = new Point(0, 0);
            var p11 = new Point(0, 0);
            var p2 = new Point(0, 1);
            Assert.IsNotNull(Point.Calculator);

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
            var p11 = new Point(3, 4);
            var p2 = new Point(6, 8);
            TestCalculator(Point.Calculator, null, p1, p11, p2, 
                new Point(9, 12), new Point(-3, -4), 5f, 5f);
        }

        private static void TestCalculator<T>(ICalculator<T> c, T @null, T a, T equalToA, T b, T aPlusB,
            T aMinusB, float lengthOfA, float distAb) where T : IEquatable<T>
        {
            Assert.IsTrue(c.Equals(a, equalToA));
            Assert.IsFalse(c.Equals(a, b));

            Assert.IsTrue(c.Add(a, b).Equals(aPlusB));
            Assert.IsFalse(c.Add(a, b).Equals(aMinusB));
            Assert.Catch(() => c.Add(a, @null));
            Assert.Catch(() => c.Add(@null, a));
            
            Assert.IsTrue(c.Subtract(a, b).Equals(aMinusB));
            Assert.IsFalse(c.Subtract(a, b).Equals(aPlusB));
            Assert.Catch(() => c.Subtract(a, @null));
            Assert.Catch(() => c.Subtract(@null, a));
            
            Assert.IsTrue(Math.Abs(c.Distance(a, b) - distAb) < float.Epsilon);
            Assert.Catch(() => c.Distance(a, @null));
            Assert.Catch(() => c.Distance(@null, a));

            Assert.IsTrue(Math.Abs(c.Length(a) - lengthOfA) < float.Epsilon);
            Assert.Catch(() => c.Length(@null));
        }
    }
}