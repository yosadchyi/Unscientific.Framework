using Unscientific.FixedPoint;

namespace Unscientific.ECS.Modules.Physics
{
    public class Intersector
    {
        public static bool CirclesIntersects(ref FixVec2 c1, Fix r1, ref FixVec2 c2, Fix r2)
        {
            var distSqr = (c2 - c1).MagnitudeSqr;
            var range = r1 + r2;

            return distSqr < range * range;
        }

        public static bool AABBIntersects(ref AABB aabb1, ref AABB aabb2)
        {
            return aabb1.Intersects(ref aabb2);
        }


        public static bool AABBIntersectsCircle(ref AABB aabb, ref FixVec2 center, Fix radius)
        {
            return aabb.SquaredDistToNearestAABBPoint(center) <= radius * radius;
        }
    }
}
