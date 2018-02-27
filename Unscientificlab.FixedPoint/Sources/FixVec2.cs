/* FixedPointy - A simple fixed-point math library for C#.
 * 
 * Copyright (c) 2013 Jameson Ernst
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */


namespace Unscientificlab.FixedPoint
{
	public struct FixVec2
	{
		public static readonly FixVec2 Zero = new FixVec2 ();
		public static readonly FixVec2 One = new FixVec2 (1, 1);
		public static readonly FixVec2 UnitX = new FixVec2 (1, 0);
		public static readonly FixVec2 UnitY = new FixVec2 (0, 1);

		public static FixVec2 operator + (FixVec2 rhs)
		{
			return rhs;
		}

		public static FixVec2 operator - (FixVec2 rhs)
		{
			return new FixVec2 (-rhs._x, -rhs._y);
		}

		public static FixVec2 operator + (FixVec2 lhs, FixVec2 rhs)
		{
			return new FixVec2 (lhs._x + rhs._x, lhs._y + rhs._y);
		}

		public static bool operator == (FixVec2 lhs, FixVec2 rhs)
		{
			return lhs._x == rhs._x && lhs._y == rhs._y;
		}

		public static bool operator != (FixVec2 lhs, FixVec2 rhs)
		{
			return lhs._x != rhs._x || lhs._y != rhs._y;
		}

		public static FixVec2 operator - (FixVec2 lhs, FixVec2 rhs)
		{
			return new FixVec2 (lhs._x - rhs._x, lhs._y - rhs._y);
		}

		public static FixVec2 operator + (FixVec2 lhs, Fix rhs)
		{
			return lhs.ScalarAdd (rhs);
		}

		public static FixVec2 operator + (Fix lhs, FixVec2 rhs)
		{
			return rhs.ScalarAdd (lhs);
		}

		public static FixVec2 operator - (FixVec2 lhs, Fix rhs)
		{
			return new FixVec2 (lhs._x - rhs, lhs._y - rhs);
		}

		public static FixVec2 operator * (FixVec2 lhs, Fix rhs)
		{
			return lhs.ScalarMultiply (rhs);
		}

		public static FixVec2 operator * (Fix lhs, FixVec2 rhs)
		{
			return rhs.ScalarMultiply (lhs);
		}

		public static FixVec2 operator / (FixVec2 lhs, Fix rhs)
		{
			return new FixVec2 (lhs._x / rhs, lhs._y / rhs);
		}

	    private Fix _x, _y;

		public FixVec2 (Fix x, Fix y)
		{
			_x = x;
			_y = y;
		}

		public Fix X { 
			get { return _x; }
			set { _x = value; }
		}

		public Fix Y { 
			get { return _y; }
			set { _y = value; }
		}

		public Fix Dot (FixVec2 rhs)
		{
			return _x * rhs._x + _y * rhs._y;
		}

		public Fix Cross (FixVec2 rhs)
		{
			return _x * rhs._y - _y * rhs._x;
		}

	    private FixVec2 ScalarAdd (Fix value)
		{
			return new FixVec2 (_x + value, _y + value);
		}

	    private FixVec2 ScalarMultiply (Fix value)
		{
			return new FixVec2 (_x * value, _y * value);
		}

		public Fix MagnitudeSqr {
			get {
				if (_x == 0 && _y == 0)
					return Fix.Zero;

				return _x * _x + _y * _y;
			}
		}

		public Fix Magnitude {
			get {
				if (_x == 0 && _y == 0)
					return Fix.Zero;

			    return FixMath.Sqrt(MagnitudeSqr);
			}
		}

		public void Normalize ()
		{
			if (_x == 0 && _y == 0)
				return;

			var m = Magnitude;

			_x /= m;
			_y /= m;
		}

		public FixVec2 Normalized {
			get {
				if (_x == 0 && _y == 0)
					return Zero;
				
				var m = Magnitude;

				return new FixVec2 (_x / m, _y / m);
			}
		}

		public static FixVec2 ClampMagnitude (FixVec2 vector, Fix maxMagnitude)
		{
			ClampMagnitude(ref vector, maxMagnitude);
			return vector;
		}

		public static void ClampMagnitude (ref FixVec2 vector, Fix maxMagnitude)
		{
			var magnitude = vector.Magnitude;

			if (magnitude > maxMagnitude) {
				vector.Normalize ();
				vector *= maxMagnitude;
			}
		}

		public override string ToString ()
		{
			return string.Format ("({0}, {1})", _x, _y);
		}

		public override int GetHashCode ()
		{
			return X.Raw.GetHashCode() ^ Y.Raw.GetHashCode();
		}

		public override bool Equals (object obj)
		{
			// Check for null values and compare run-time types.
			if (obj == null || GetType () != obj.GetType ())
				return false;

			var v = (FixVec2)obj;

		    return (X == v.X) && (Y == v.Y);
		}

	    public static FixVec2 Lerp(FixVec2 v1, FixVec2 v2, float t)
	    {
	        return v1 * (Fix.One - t) + v2 * t;
	    }

		public void Sub(ref FixVec2 b)
		{
			_x -= b._x;
			_y -= b._y;
		}

		public void Scale(ref Fix scale)
		{
			_x *= scale;
			_y *= scale;
		}
	}
}
