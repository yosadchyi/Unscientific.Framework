using Unscientific.FixedPoint;
using Unscientific.Util.Pool;

namespace Unscientific.ECS.Modules.Physics.Shapes
{
    public class AABBShape: Shape
    {
        public readonly Fix Width;
        public readonly Fix Height;

        public override ShapeType Type => ShapeType.AABB;

        public AABBShape(Fix width, Fix height)
        {
            Width = width;
            Height = height;
        }

        public override AABB GetBoundingBox(ref Transform transform)
        {
            return new AABB(transform.Position, Width / 2, Height / 2);
        }

    }
}