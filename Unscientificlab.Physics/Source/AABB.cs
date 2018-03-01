/* Copyright (c) 2007 Scott Lembcke ported by Jose Medrano (@netonjm)

  Permission is hereby granted, free of charge, to any person obtaining a copy
  of this software and associated documentation files (the "Software"), to deal
  in the Software without restriction, including without limitation the rights
  to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
  copies of the Software, and to permit persons to whom the Software is
  furnished to do so, subject to the following conditions:

  The above copyright notice and this permission notice shall be included in
  all copies or substantial portions of the Software.

  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
  SOFTWARE.
 */

using Unscientificlab.FixedPoint;

namespace Unscientificlab.Physics
{
    public struct AABB
    {
        public Fix L;
        public Fix B;
        public Fix R;
        public Fix T;

        public AABB(Fix l, Fix b, Fix r, Fix t)
        {
            L = l;
            B = b;
            R = r;
            T = t;
        }

        public AABB(FixVec2 p, Fix r)
            : this(p.X - r, p.Y - r, p.X + r, p.Y + r)
        {
        }

        /// Constructs a cpBB centered on a point with the given extents (half sizes).
        public AABB(FixVec2 c, Fix hwMax, Fix hhMax) : this(c.X - hwMax, c.Y - hhMax, c.X + hwMax, c.Y + hhMax)
        {
        }

        public override string ToString()
        {
            return string.Format("l:{0},b:{1},r:{2},t:{3}", L, B, R, T);
        }

        /// Constructs a cpBB for a circle with the given position and radius.
        public static AABB NewForCircle(FixVec2 p, Fix r)
        {
            return new AABB(p.X - r, p.Y - r, p.X + r, p.Y + r);
        }

        public Fix Proximity(AABB b)
        {
            return Proximity(this, b);
        }

        public static Fix Proximity(AABB a, AABB b)
        {
            return FixMath.Abs(a.L + a.R - b.L - b.R) + FixMath.Abs(a.B + a.T - b.B - b.T);
        }

        public bool Overlaps(ref AABB aabb2)
        {
            if (R < aabb2.L || T < aabb2.B) return false;
            if (L > aabb2.R || B > aabb2.T) return false;
            return true;
        }

        /// Returns true if @c a and @c b intersect.
        public bool Intersects(ref AABB a)
        {
            return Intersects(ref this, ref a);
        }

        public static bool Intersects(ref AABB a, ref AABB b)
        {
            return (a.L <= b.R && b.L <= a.R && a.B <= b.T && b.B <= a.T);
        }

        /// Returns true if @c other lies completely within @c bb.
        public bool ContainsBB(ref AABB other)
        {
            return ContainsBB(ref this, ref other);
        }

        public static bool ContainsBB(ref AABB aabb, ref AABB other)
        {
            return (aabb.L <= other.L && aabb.R >= other.R && aabb.B <= other.B && aabb.T >= other.T);
        }

        /// Returns true if @c bb contains @c v.
        public bool ContainsVect(FixVec2 v)
        {
            return ContainsVect(this, v);
        }

        public static bool ContainsVect(AABB aabb, FixVec2 v)
        {
            return (aabb.L <= v.X && aabb.R >= v.X && aabb.B <= v.Y && aabb.T >= v.Y);
        }

        /// Returns a bounding box that holds both bounding boxes.
        public AABB Merge(AABB a)
        {
            return Merge(this, a);
        }

        public static AABB Merge(AABB a, AABB b)
        {
            return new AABB(
                FixMath.Min(a.L, b.L),
                FixMath.Min(a.B, b.B),
                FixMath.Max(a.R, b.R),
                FixMath.Max(a.T, b.T)
            );
        }

        /// Returns a bounding box that holds both @c bb and @c v.
        public AABB Expand(FixVec2 v)
        {
            return Expand(this, v);
        }

        public static AABB Expand(AABB aabb, FixVec2 v)
        {
            return new AABB(
                FixMath.Min(aabb.L, v.X),
                FixMath.Min(aabb.B, v.Y),
                FixMath.Max(aabb.R, v.X),
                FixMath.Max(aabb.T, v.Y)
            );
        }

        /// Returns the center of a bounding box.
        public static FixVec2 Center(AABB aabb)
        {
            return FixVec2.Lerp(new FixVec2(aabb.L, aabb.B), new FixVec2(aabb.R, aabb.T), 0.5f);
        }

        public FixVec2 Center()
        {
            return Center(this);
        }


        /// Returns the area of the bounding box.
        public static Fix Area(AABB aabb)
        {
            return (aabb.R - aabb.L) * (aabb.T - aabb.B);
        }

        public Fix Area()
        {
            return Area(this);
        }


        /// Merges @c a and @c b and returns the area of the merged bounding box.
        public static Fix MergedArea(AABB a, AABB b)
        {
            return (FixMath.Max(a.R, b.R) - FixMath.Min(a.L, b.L)) * (FixMath.Max(a.T, b.T) - FixMath.Min(a.B, b.B));
        }

        public Fix MergedArea(AABB a)
        {
            return MergedArea(this, a);
        }


        /// Returns the fraction along the segment query the cpBB is hit. Returns INFINITY if it doesn't hit.
        public static Fix SegmentQuery(ref AABB aabb, ref FixVec2 a, ref FixVec2 b)
        {
            var idx = 1.0f / (b.X - a.X);
            var tx1 = (aabb.L == a.X ? Fix.MinValue : (aabb.L - a.X) * idx);
            var tx2 = (aabb.R == a.X ? Fix.MaxValue : (aabb.R - a.X) * idx);
            var txmin = FixMath.Min(tx1, tx2);
            var txmax = FixMath.Max(tx1, tx2);

            var idy = 1.0f / (b.Y - a.Y);
            var ty1 = (aabb.B == a.Y ? Fix.MinValue : (aabb.B - a.Y) * idy);
            var ty2 = (aabb.T == a.Y ? Fix.MaxValue : (aabb.T - a.Y) * idy);
            var tymin = FixMath.Min(ty1, ty2);
            var tymax = FixMath.Max(ty1, ty2);

            if (tymin <= txmax && txmin <= tymax)
            {
                var min = FixMath.Max(txmin, tymin);
                var max = FixMath.Min(txmax, tymax);

                if (0 <= max && min <= 1) return FixMath.Max(min, 0);
            }

            return Fix.MaxValue;
        }

        public Fix SegmentQuery(FixVec2 a, FixVec2 b)
        {
            return SegmentQuery(ref this, ref a, ref b);
        }


        public bool IntersectsSegment(FixVec2 a, FixVec2 b)
        {
            return SegmentQuery(ref this, ref a, ref b) != Fix.MaxValue;
        }


        /// Clamp a vector to a bounding box.
        public FixVec2 ClampVect(FixVec2 v)
        {
            return new FixVec2(FixMath.Clamp(v.X, L, R), FixMath.Clamp(v.Y, B, T));
        }


        /// Wrap a vector to a bounding box.
        public static FixVec2 WrapVect(AABB aabb, FixVec2 v)
        {
            // wrap a vector to a bbox
            var ix = FixMath.Abs(aabb.R - aabb.L);
            var modx = (v.X - aabb.L) % ix;
            var x = (modx > 0) ? modx : modx + ix;

            var iy = FixMath.Abs(aabb.T - aabb.B);
            var mody = (v.Y - aabb.B) % iy;
            var y = (mody > 0) ? mody : mody + iy;

            return new FixVec2(x + aabb.L, y + aabb.B);
        }

        public Fix SquaredDistToNearestAABBPoint(FixVec2 point) {
            var result = Fix.Zero;

            var v = point.X;

            if (v < L) result += (L - v) * (L - v);
            if (v > R) result += (v - R) * (v - R);

            v = point.Y;

            if (v < B) result += (B - v) * (B - v);
            if (v > T) result += (v - T) * (v - T);

            return result;
        }

    }
}