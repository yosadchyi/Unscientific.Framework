using NUnit.Framework;

namespace Unscientific.FixedPoint.Tests
{
    public static class FixAssert
    {
        public static void AssertEquals(Fix actual, float expected, float Epsilon)
        {
            Assert.That(actual.AsFloat, Is.EqualTo(expected).Within(Epsilon));
        }
    }
}