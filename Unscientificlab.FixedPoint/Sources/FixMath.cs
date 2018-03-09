using System;

namespace Unscientificlab.FixedPoint
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

//        public static Fix Sin(Fix i)
//        {
//            Fix j = 0;
//            for (; i < 0; i += Fix.Create(25736, false))
//                ;
//            if (i > Fix.Create(25736, false))
//                i %= Fix.Create(25736, false);
//            var k = (i * Fix.Create(10, false)) / Fix.Create(714, false);
//            if (i != 0 && i != Fix.Create(6434, false) && i != Fix.Create(12868, false) &&
//                i != Fix.Create(19302, false) && i != Fix.Create(25736, false))
//                j = (i * Fix.Create(100, false)) / Fix.Create(714, false) - k * Fix.Create(10, false);
//            if (k <= Fix.Create(90, false))
//                return SinLookup(k, j);
//            if (k <= Fix.Create(180, false))
//                return SinLookup(Fix.Create(180, false) - k, j);
//            if (k <= Fix.Create(270, false))
//                return SinLookup(k - Fix.Create(180, false), j).Inverse;
//            return SinLookup(Fix.Create(360, false) - k, j).Inverse;
//        }
//
//        private static Fix SinLookup(Fix i, Fix j)
//        {
//            if (j > 0 && j < Fix.Create(10, false) && i < Fix.Create(90, false))
//                return Fix.Create(SinTable[i.RawValue], false) +
//                       ((Fix.Create(SinTable[i.RawValue + 1], false) - Fix.Create(SinTable[i.RawValue], false)) /
//                        Fix.Create(10, false)) * j;
//            return Fix.Create(SinTable[i.RawValue], false);
//        }
//
//        private static readonly int[] SinTable =
//        {
//            0, 71, 142, 214, 285, 357, 428, 499, 570, 641,
//            711, 781, 851, 921, 990, 1060, 1128, 1197, 1265, 1333,
//            1400, 1468, 1534, 1600, 1665, 1730, 1795, 1859, 1922, 1985,
//            2048, 2109, 2170, 2230, 2290, 2349, 2407, 2464, 2521, 2577,
//            2632, 2686, 2740, 2793, 2845, 2896, 2946, 2995, 3043, 3091,
//            3137, 3183, 3227, 3271, 3313, 3355, 3395, 3434, 3473, 3510,
//            3547, 3582, 3616, 3649, 3681, 3712, 3741, 3770, 3797, 3823,
//            3849, 3872, 3895, 3917, 3937, 3956, 3974, 3991, 4006, 4020,
//            4033, 4045, 4056, 4065, 4073, 4080, 4086, 4090, 4093, 4095,
//            4096
//        };
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