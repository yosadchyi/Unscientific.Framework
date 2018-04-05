// The MIT License (MIT)
//
// Copyright (c) 2013 Jacob Dufault
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and
// associated documentation files (the "Software"), to deal in the Software without restriction,
// including without limitation the rights to use, copy, modify, merge, publish, distribute,
// sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or
// substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT
// NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System;

namespace Unscientific.FixedPoint
{
    /// <summary>
    /// A Real value implements floating point operations on the CPU. It does not adhere any any
    /// IEEE standard, but has the extremely important attribute of providing identical semantics on
    /// every CPU which executes it. This is otherwise impossible to guarantee in the CLR,
    /// especially when 3rd party code is running and/or C++ DLL access is unavailable to set x87
    /// FPU rounding modes.
    /// </summary>
    /// <remarks>
    /// This class has been adapted from http://stackoverflow.com/a/616015.
    /// </remarks>
    // TODO: The Real public API needs to be trimmed before the 1.0 release; implementation details
    //       are currently way too public
    public struct Fix
    {
        public int Raw;
        public const int ShiftAmount = 12; //12 is 4096
        public const int IntegerMask = (1 << ShiftAmount) - 1;

        public const int OneI = 1 << ShiftAmount;
        public static Fix Zero = new Fix();
        public static Fix One = Create(1, true);

        public static Fix MaxValue = CreateFromRaw(int.MaxValue);
        public static Fix MinValue = CreateFromRaw(int.MinValue);

        #region Constructors

        public static implicit operator Fix(float value)
        {
            return Create(value);
        }

        public static implicit operator Fix(double value)
        {
            return Create((float) value);
        }

        public static Fix CreateFromRaw(int startingRawValue)
        {
            Fix fix;
            fix.Raw = startingRawValue;
            return fix;
        }

        /// <summary>
        /// Assuming this real has a base 10 representation, this shifts the decimal value to the
        /// left by count digits.
        /// </summary>
        private void ShiftDecimal(int count)
        {
            const int digitBase = 10;

            var pow = 1;
            for (var i = 0; i < count; ++i)
            {
                pow *= digitBase;
            }

            this /= (One * pow);
        }

        /// <summary>
        /// Creates a real value with that is 0.number. For example, CreateDecimal(123) will create
        /// a real value that is equal to "0.123".
        /// </summary>
        /// <remarks>
        /// CreateDecimal(1, 0005, 4) will create 1.0005 CreateDecimal(1, 5, 4) will create 1.0005
        /// </remarks>
        /// <returns></returns>
        public static Fix CreateDecimal(int beforeDecimal, int afterDecimal, int afterDigits)
        {
            var sign = beforeDecimal >= 0 ? 1 : -1;

            Fix fix;
            fix.Raw = (OneI * afterDecimal) * sign;
            fix.ShiftDecimal(afterDigits);
            fix.Raw += OneI * beforeDecimal;
            return fix;
        }

        public static Fix CreateDecimal(int beforeDecimal)
        {
            Fix fix;
            fix.Raw = OneI * beforeDecimal;
            return fix;
        }

        public static Fix Create(int startingRawValue, bool useMultiple)
        {
            Fix fInt;
            fInt.Raw = startingRawValue;
            if (useMultiple)
                fInt.Raw = fInt.Raw << ShiftAmount;
            return fInt;
        }

        public static Fix Create(double doubleValue)
        {
            Fix fInt;
            doubleValue *= OneI;
            fInt.Raw = (int) Math.Round(doubleValue);
            return fInt;
        }

        public static Fix Ratio(int a, int b)
        {
            return (Fix) a / b;
        }

        #endregion

        public int AsInt => (int) (Raw >> ShiftAmount);

        public float AsFloat => Raw / (float) OneI;

        public double AsDouble => Raw / (double) OneI;

        public Fix Inverse => Create(-Raw, false);

        #region FromParts

        /// <summary>
        /// Create a fixed-int number from parts. For example, to create 1.5 pass in 1 and 500.
        /// </summary>
        /// <param name="preDecimal">The number above the decimal. For 1.5, this would be 1.</param>
        /// <param name="postDecimal">The number below the decimal, to three digits. For 1.5, this
        /// would be 500. For 1.005, this would be 5.</param>
        /// <returns>A fixed-int representation of the number parts</returns>
        public static Fix FromParts(int preDecimal, int postDecimal)
        {
            var f = Create(preDecimal, true);
            if (postDecimal != 0)
                f.Raw += (Create(postDecimal) / 1000).Raw;

            return f;
        }

        #endregion

        #region *

        public static Fix operator *(Fix one, Fix other)
        {
            Fix fInt;
            long oneRaw = one.Raw;
            long otherRaw = other.Raw;
            fInt.Raw = (int) ((oneRaw * otherRaw) >> ShiftAmount);
            return fInt;
        }

        public static Fix operator *(Fix one, int multi)
        {
            return one * (Fix) multi;
        }

        public static Fix operator *(int multi, Fix one)
        {
            return one * (Fix) multi;
        }

        #endregion

        #region /

        public static Fix operator /(Fix one, Fix other)
        {
            Fix fInt;
            long oneRaw = one.Raw;
            fInt.Raw = (int) ((oneRaw << ShiftAmount) / (other.Raw));
            return fInt;
        }

        public static Fix operator /(Fix one, int divisor)
        {
            return one / (Fix) divisor;
        }

        public static Fix operator /(int divisor, Fix one)
        {
            return (Fix) divisor / one;
        }

        #endregion

        #region %

        public static Fix operator %(Fix one, Fix other)
        {
            Fix fInt;
            fInt.Raw = (one.Raw) % (other.Raw);
            return fInt;
        }

        public static Fix operator %(Fix one, int divisor)
        {
            return one % (Fix) divisor;
        }

        public static Fix operator %(int divisor, Fix one)
        {
            return (Fix) divisor % one;
        }

        #endregion

        #region +

        public static Fix operator +(Fix one, Fix other)
        {
            Fix fInt;
            fInt.Raw = one.Raw + other.Raw;
            return fInt;
        }

        public static Fix operator +(Fix one, int other)
        {
            return one + (Fix) other;
        }

        public static Fix operator +(int other, Fix one)
        {
            return one + (Fix) other;
        }

        #endregion

        #region -

        public static Fix operator -(Fix a)
        {
            return a.Inverse;
        }

        public static Fix operator -(Fix one, Fix other)
        {
            Fix fInt;
            fInt.Raw = one.Raw - other.Raw;
            return fInt;
        }

        public static Fix operator -(Fix one, int other)
        {
            return one - (Fix) other;
        }

        public static Fix operator -(int other, Fix one)
        {
            return (Fix) other - one;
        }

        #endregion

        #region ==

        public static bool operator ==(Fix one, Fix other)
        {
            return one.Raw == other.Raw;
        }

        public static bool operator ==(Fix one, int other)
        {
            return one == (Fix) other;
        }

        public static bool operator ==(int other, Fix one)
        {
            return (Fix) other == one;
        }

        #endregion

        #region !=

        public static bool operator !=(Fix one, Fix other)
        {
            return one.Raw != other.Raw;
        }

        public static bool operator !=(Fix one, int other)
        {
            return one != (Fix) other;
        }

        public static bool operator !=(int other, Fix one)
        {
            return (Fix) other != one;
        }

        #endregion

        #region >=

        public static bool operator >=(Fix one, Fix other)
        {
            return one.Raw >= other.Raw;
        }

        public static bool operator >=(Fix one, int other)
        {
            return one >= (Fix) other;
        }

        public static bool operator >=(int other, Fix one)
        {
            return (Fix) other >= one;
        }

        #endregion

        #region <=

        public static bool operator <=(Fix one, Fix other)
        {
            return one.Raw <= other.Raw;
        }

        public static bool operator <=(Fix one, int other)
        {
            return one <= (Fix) other;
        }

        public static bool operator <=(int other, Fix one)
        {
            return (Fix) other <= one;
        }

        #endregion

        #region >

        public static bool operator >(Fix one, Fix other)
        {
            return one.Raw > other.Raw;
        }

        public static bool operator >(Fix one, int other)
        {
            return one > (Fix) other;
        }

        public static bool operator >(int other, Fix one)
        {
            return (Fix) other > one;
        }

        #endregion

        #region <

        public static bool operator <(Fix one, Fix other)
        {
            return one.Raw < other.Raw;
        }

        public static bool operator <(Fix one, int other)
        {
            return one < (Fix) other;
        }

        public static bool operator <(int other, Fix one)
        {
            return (Fix) other < one;
        }

        #endregion

        public static explicit operator int(Fix src)
        {
            return (int) (src.Raw >> ShiftAmount);
        }

        public static explicit operator float(Fix src)
        {
            return src.Raw / (float) OneI;
        }

        public static explicit operator double(Fix src)
        {
            return src.Raw / (double) OneI;
        }

        public static explicit operator Fix(int src)
        {
            return Create(src, true);
        }

        public static explicit operator Fix(long src)
        {
            return Create((int) src, true);
        }

        public static explicit operator Fix(ulong src)
        {
            return Create((int) src, true);
        }

        public static Fix operator <<(Fix one, int amount)
        {
            return Create(one.Raw << amount, false);
        }

        public static Fix operator >>(Fix one, int amount)
        {
            return Create(one.Raw >> amount, false);
        }

        public override bool Equals(object obj)
        {
            if (obj is Fix)
                return ((Fix) obj).Raw == Raw;
            return false;
        }

        public override int GetHashCode()
        {
            return Raw.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("{0}", AsFloat);
        }

        public static Fix Mul(Fix f1, Fix f2)
        {
            return f1 * f2;
        }
    }
}