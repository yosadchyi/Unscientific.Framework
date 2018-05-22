using Unscientific.FixedPoint;

namespace Unscientific.ECS.Features.Physics2D.Shapes
{
    public class CircleShape: Shape
    {
        public readonly Fix Radius;
        public override ShapeType Type => ShapeType.Circle;

        public CircleShape(Fix radius)
        {
            Radius = radius;
        }

        public override AABB GetBoundingBox(ref Transform transform)
        {
            return new AABB(transform.Position, Radius);
        }
    }
}