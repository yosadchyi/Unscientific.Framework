using System;
using NUnit.Framework;
using Unscientificlab.FixedPoint;

namespace Assets.Tests.Unscientificlab.FixedPoint
{
    public class FixMathTests
    {
        public const float Epsilon = 0.001f;

        [Test]
        public void SinAverageDeviationTest()
        {
            var totalDiff = 0.0;
            var maxDiff = double.MinValue;

            for (var i = 0; i < 3600; i++)
            {
                var fixAngle = FixMath.Pi * i / 1800;
                var doubleAngle = Math.PI * i / 1800.0;

                var diff = Math.Abs((double) FixMath.Sin(fixAngle) - Math.Sin(doubleAngle));
                totalDiff += diff;
                maxDiff = Math.Max(maxDiff, Math.Abs(diff));
            }
            // Max & Average deviation
            FixAssert.AssertEquals(maxDiff, 0, 0.019f);
            FixAssert.AssertEquals(totalDiff / 360, 0, 0.07f);
        }

        [Test]
        public void CosAverageDeviationTest()
        {
            var totalDiff = 0.0;
            var maxDiff = double.MinValue;

            for (var i = 0; i < 3600; i++)
            {
                var fixAngle = FixMath.Pi * i / 1800;
                var doubleAngle = Math.PI * i / 1800.0;

                var diff = (double) FixMath.Cos(fixAngle) - Math.Cos(doubleAngle);
                totalDiff += diff;
                maxDiff = Math.Max(maxDiff, Math.Abs(diff));
            }
            // Max & Average deviation
            FixAssert.AssertEquals(maxDiff, 0, 0.019f);
            FixAssert.AssertEquals(totalDiff / 360, 0, 0.007f);
        }

        [Test]
        public void AsinAverageDeviationTest()
        {
            var totalDiff = 0.0;
            var maxDiff = double.MinValue;

            for (var i = 0; i < 1000; i++)
            {
                var fixValue = Fix.Ratio(i, 1000);
                var doubleValue = i / 1000.0;

                var diff = Math.Abs((double) FixMath.Asin(fixValue) - Math.Asin(doubleValue));
                totalDiff += diff;
                maxDiff = Math.Max(maxDiff, Math.Abs(diff));
            }
            // Max & Average deviation
            FixAssert.AssertEquals(maxDiff, 0, 0.005f);
            FixAssert.AssertEquals(totalDiff / 360, 0, 0.002f);
        }

        [Test]
        public void AtanAverageDeviationTest()
        {
            var totalDiff = 0.0;
            var maxDiff = double.MinValue;

            for (var i = 0; i < 100; i++)
            {
                for (var j = 0; j < 100; j++)
                {

                    var diff = (double) FixMath.Atan2(i, j) - Math.Atan2(i, j);
                    totalDiff += diff;
                    maxDiff = Math.Max(maxDiff, Math.Abs(diff));
                }
            }
            // Max & Average deviation
            FixAssert.AssertEquals(maxDiff, 0, 0.012f);
            FixAssert.AssertEquals(totalDiff / 360, 0, 0.008f);
        }

        [Test]
        public void SqrtAverageDeviationTest()
        {
            var maxDiff = double.MinValue;

            for (var i = 0; i <= 1000; i++)
            {
                var diff = FixMath.Sqrt(i).AsDouble - Math.Sqrt(i);

                maxDiff = Math.Max(maxDiff, Math.Abs(diff));
            }

            FixAssert.AssertEquals(maxDiff, 0, Epsilon);
        }
    }
}