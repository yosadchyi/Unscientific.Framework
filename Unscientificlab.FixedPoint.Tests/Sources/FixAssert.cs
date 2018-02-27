using NUnit.Framework;
using Unscientificlab.FixedPoint;

namespace Unscientificlab.FixedPoint
{
    public static class FixAssert
    {
        public static void AssertEquals(Fix actual, float expected, float Epsilon)
        {
            Assert.That(actual.AsFloat, Is.EqualTo(expected).Within(Epsilon));
        }
    }
}