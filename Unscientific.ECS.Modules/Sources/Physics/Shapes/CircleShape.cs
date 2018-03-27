using Unscientific.FixedPoint;
using Unscientific.Util.Pool;

namespace Unscientific.ECS.Modules.Physics.Shapes
{
    public class CircleShape: Shape
    {
        public static ObjectPool<CircleShape> Pool = new GenericObjectPool<CircleShape>(128);

        public static CircleShape New(Fix radius)
        {
            var circleShape = Pool.Get();
            circleShape.Radius = radius;
            return circleShape;
        }

        public override void Return()
        {
            Pool.Return(this);
        }

        public Fix Radius;

        public override ShapeType Type => ShapeType.Circle;

        public override AABB GetBoundingBox(ref Transform transform)
        {
            return new AABB(transform.Position, Radius);
        }
    }
}