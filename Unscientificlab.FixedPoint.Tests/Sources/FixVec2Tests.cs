using System;
using NUnit.Framework;

namespace Unscientific.FixedPoint.Tests
{
    public class FixVec2Tests
    {
        private const float Epsilon = 1.0f / 4096.0f;

        [Test]
        public void FixVec2MagnitudeTest()
        {
            var fixVec2 = new FixVec2(2, 1);

            FixAssert.AssertEquals(fixVec2.Magnitude, (float) Math.Sqrt(2 * 2 + 1 * 1), Epsilon);
        }
    }
}