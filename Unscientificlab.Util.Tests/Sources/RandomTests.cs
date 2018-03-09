using System.Linq;
using NUnit.Framework;
using Random = Unscientificlab.Util.Random;

namespace Unscientificlab.Util.Tests
{
    [TestFixture]
    public class RandomTests
    {
        private const ulong Seed = 23473418134;
        private readonly int[] _first10 = {2108429564, 1240136508, 128561491, 1902245342, 1734340080, 1232912945, 1846484120, 1762126024, 105454178, 467479315};
        
        [Test]
        public void NextShouldReturnDeterministicSequence()
        {
            var random = new Random(Seed);
            var enumerable = from v in Enumerable.Range(0, 10) select random.Next();
            var numbers = enumerable.ToList();

            Assert.AreEqual(numbers, _first10);
        }

        [Test]
        public void CallingSeedShouldResetSequence()
        {
            var random = new Random(Seed);
            var enumerable = from v in Enumerable.Range(0, 10) select random.Next();
            var numbers = enumerable.ToList();

            Assert.AreEqual(numbers, _first10);
            random.Seed(Seed);
            enumerable = from v in Enumerable.Range(0, 10) select random.Next();
            numbers = enumerable.ToList();
            Assert.AreEqual(numbers, _first10);
        }

        [Test]
        public void NextWithMaxShouldReturnNumbersBetweenZeroAndMax()
        {
            var random = new Random(Seed);
            const int max = 10;
            var enumerable = from v in Enumerable.Range(0, 1000) select random.Next(max);

            foreach (var v in enumerable)
            {
                Assert.GreaterOrEqual(v, 0);
                Assert.Less(v, max);
            }
        }
        
        [Test]
        public void NextWithMinMaxShouldReturnNumbersBetweenMinAndMax()
        {
            var random = new Random(Seed);
            const int min = 5;
            const int max = 10;
            var enumerable = from v in Enumerable.Range(0, 1000) select random.Next(min, max);

            foreach (var v in enumerable)
            {
                Assert.GreaterOrEqual(v, min);
                Assert.Less(v, max);
            }
        }

    }
}