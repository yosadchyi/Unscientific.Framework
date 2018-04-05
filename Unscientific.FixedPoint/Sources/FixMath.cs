using System;

namespace Unscientific.FixedPoint
{
    public static class FixMath
    {
        public static Fix Pi = Fix.Create(12868, false); //PI x 2^12
        public static Fix Pi2 = Pi * 2; //radian equivalent of 360 degrees
        public static Fix Deg2Rad = Pi / (Fix) 180; //PI / 180
        public static Fix Epsilon = Fix.CreateFromRaw(1);

        public static Fix Sqrt(Fix f, int numberOfIterations)
        {
            if (f.Raw < 0) //NaN in Math.Sqrt
                throw new ArithmeticException("Input Error");
            if (f.Raw == 0)
                return 0;
            var k = f + Fix.One >> 1;
            for (var i = 0; i < numberOfIterations; i++)
                k = (k + (f / k)) >> 1;

            if (k.Raw < 0)
                throw new ArithmeticException("Overflow");
            return k;
        }

        public static Fix Sqrt(Fix f)
        {
            byte numberOfIterations = 8;
            if (f.Raw > 0x64000)
                numberOfIterations = 12;
            if (f.Raw > 0x3e8000)
                numberOfIterations = 16;
            return Sqrt(f, numberOfIterations);
        }

        private static readonly Fix Fix360 = Fix.CreateFromRaw(360 * (1 << Fix.ShiftAmount));

        public static Fix Sin(Fix angle)
        {
            var degrees = (angle * 180) / Pi;

            while (degrees < 0)
                degrees += Fix360;

            if (degrees > Fix360)
                degrees %= Fix360;

            var index = degrees.AsInt * 4096 / 360;

            return Fix.CreateFromRaw(FixConst.SinTable[index]);
        }

        public static Fix Cos(Fix i)
        {
            return Sin(i + Fix.Create(6435, false));
        }

        public static Fix Tan(Fix i)
        {
            return Sin(i) / Cos(i);
        }

        public static Fix Acos(Fix f)
        {
            return Asin(Sqrt(1 - f * f));
        }

        public static Fix Asin(Fix f)
        {
            var isNegative = f < 0;
            f = Abs(f);

            if (f > Fix.One)
                throw new ArithmeticException("Bad Asin Input:" + f.AsFloat);

            var f1 = Fix.Mul(Fix.Mul(Fix.Mul(Fix.Mul(Fix.Create(145103 >> Fix.ShiftAmount, false), f) -
                                 Fix.Create(599880 >> Fix.ShiftAmount, false), f) +
                             Fix.Create(1420468 >> Fix.ShiftAmount, false), f) -
                         Fix.Create(3592413 >> Fix.ShiftAmount, false), f) +
                     Fix.Create(26353447 >> Fix.ShiftAmount, false);
            var f2 = Pi / Fix.Create(2, true) - (Sqrt(Fix.One - f) * f1);

            return isNegative ? f2.Inverse : f2;
        }

        public static Fix Atan(Fix f)
        {
            return Asin(f / Sqrt(Fix.One + (f * f)));
        }

        public static Fix Atan2(Fix f1, Fix f2)
        {
            if (f2.Raw == 0 && f1.Raw == 0)
                return 0;

            Fix result;
            if (f2 > 0)
                result = Atan(f1 / f2);
            else if (f2 < 0)
            {
                if (f1 >= 0)
                    result = (Pi - Atan(Abs(f1 / f2)));
                else
                    result = (Pi - Atan(Abs(f1 / f2))).Inverse;
            }
            else
                result = (f1 >= 0 ? Pi : Pi.Inverse) / Fix.Create(2, true);

            return result;
        }

        public static int Sign(Fix f)
        {
            if (f == 0)
                return 0;
            return f < 0 ? -1 : 1;
        }

        public static Fix Abs(Fix f)
        {
            if (f < 0)
                return f.Inverse;
            return f;
        }

        public static Fix Floor(Fix f)
        {
            Fix f2;
            f2.Raw = (f.Raw >> Fix.ShiftAmount) << Fix.ShiftAmount;
            return f2;
        }

        public static Fix Ceiling(Fix f)
        {
            Fix f2;
            f2.Raw = ((f.Raw >> Fix.ShiftAmount) << Fix.ShiftAmount) + Fix.OneI;
            return f2;
        }

        public static Fix Min(Fix one, Fix other)
        {
            return one.Raw < other.Raw ? one : other;
        }

        public static Fix Max(Fix one, Fix other)
        {
            return one.Raw > other.Raw ? one : other;
        }

        public static Fix Clamp(Fix f, Fix min, Fix max)
        {
            return Min(Max(f, min), max);
        }

        public static float Lerp(float f1, float f2, float t)
        {
            return f1 * (1.0f - t) + f2 * t;
        }
    }
}