using NUnit.Framework;
using Unscientificlab.Util.Collections;
using Unscientificlab.Utils;

namespace Unscientificlab.Utils.Tests
{
    public class DequeTests
    {
        [Test]
        public void AddFrontTest()
        {
            var deque = new Deque<object>();

            deque.AddFront(1);
            deque.AddFront(2);

            Assert.AreEqual(2, deque[0]);
            Assert.AreEqual(1, deque[1]);
        }

        [Test]
        public void AddBackTest()
        {
            var deque = new Deque<object>();

            deque.AddBack(1);
            deque.AddBack(2);

            Assert.AreEqual(1, deque[0]);
            Assert.AreEqual(2, deque[1]);
        }

        [Test]
        public void CountTest()
        {
            var deque = new Deque<object>();

            deque.AddBack(1);
            Assert.AreEqual(1, deque.Count);

            deque.AddFront(2);
            Assert.AreEqual(2, deque.Count);
        }

        [Test]
        public void RemoveFrontTest()
        {
            var deque = new Deque<object>();

            deque.AddFront(1);
            deque.AddFront(2);

            Assert.AreEqual(2, deque.RemoveFront());
            Assert.AreEqual(1, deque.RemoveFront());
        }

        [Test]
        public void RemoveBackTest()
        {
            var deque = new Deque<object>();

            deque.AddFront(1);
            deque.AddFront(2);

            Assert.AreEqual(1, deque.RemoveBack());
            Assert.AreEqual(2, deque.RemoveBack());
        }

        [Test]
        public void AddNullTest()
        {
            var deque = new Deque<object>();

            deque.AddBack(null);

            Assert.IsNull(deque.RemoveBack());
        }
    }
}