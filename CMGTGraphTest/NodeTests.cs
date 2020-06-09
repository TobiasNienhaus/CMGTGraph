using System;
using NUnit.Framework;
using Node = CMGTGraph.Algorithms.Algorithms.AStarNode<float>;
using NodeI = CMGTGraph.Algorithms.Algorithms.AStarNode<int>;

namespace CMGTGraphTest
{
    public class NodeTests
    {
        private Random _random;
        
        [SetUp]
        public void SetUp()
        {
            _random = new Random();            
        }
        
        [Test]
        public void EqualityTest()
        {
            var n1 = new Node(1f);
            var n11 = new Node(1f);
            var n2 = new Node(2f);

            Assert.IsTrue(n1.Equals(n11));
            Assert.IsTrue(n1.Equals(n1));
            Assert.IsFalse(n1.Equals(n2));

            Assert.IsFalse(n1.Equals(null));

            Assert.IsTrue(n1.Equals((object) n11));
            Assert.IsTrue(n1.Equals((object) n1));
            Assert.IsFalse(n1.Equals((object) n2));

            Assert.IsFalse(n1.Equals((object) null));

            for (var i = 0; i < 10000; i++)
            {
                var val = _random.Next();
                var a = new NodeI(val);
                var b = new NodeI(val);
                Assert.True(a.GetHashCode() == b.GetHashCode());
            }

            Assert.IsTrue(n1 == n11);
            Assert.IsFalse(n1 == n2);

            Assert.IsTrue(n1 != n2);
            Assert.IsFalse(n1 != n11);

            Assert.IsFalse(n1 == null);
        }
    }
}