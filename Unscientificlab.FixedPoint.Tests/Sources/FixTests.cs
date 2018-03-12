using NUnit.Framework;

namespace Unscientific.FixedPoint.Tests
{
    public class FixTests
    {
        public const float Epsilon = 1.0f / 4096.0f;

        [Test]
        public void CreateDecimalTest()
        {
            var fix = Fix.CreateDecimal(1, 12, 2);

            FixAssert.AssertEquals(fix, 1.12f, Epsilon);
        }

        [Test]
        public void RatioTest()
        {
            var fix = Fix.Ratio(1, 2);

            FixAssert.AssertEquals(fix, 0.5f, Epsilon);
        }

        [Test]
        public void AddTest()
        {
            var f1 = Fix.CreateDecimal(12, 25, 2);
            var f2 = Fix.CreateDecimal(123, 75, 2);

            FixAssert.AssertEquals(f1 + f2, 12.25f + 123.75f, Epsilon);
        }

        [Test]
        public void SubTest()
        {
            var f1 = Fix.CreateDecimal(12, 25, 2);
            var f2 = Fix.CreateDecimal(123, 75, 2);

            FixAssert.AssertEquals(f1 + f2, 12.25f + 123.75f, Epsilon);
        }

        [Test]
        public void DivTest()
        {
            var f1 = Fix.CreateDecimal(12, 25, 2);
            var f2 = Fix.CreateDecimal(123, 75, 2);

            FixAssert.AssertEquals(f1 / f2, 12.25f / 123.75f, Epsilon);
        }

        [Test]
        public void MulTest()
        {
            var f1 = Fix.CreateDecimal(12, 25, 2);
            var f2 = Fix.CreateDecimal(123, 75, 2);

            FixAssert.AssertEquals(f1 * f2, 12.25f * 123.75f, Epsilon);
        }
    }
}