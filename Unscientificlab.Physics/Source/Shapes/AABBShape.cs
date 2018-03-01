using Unscientificlab.FixedPoint;
using Unscientificlab.Util.Pool;

namespace Unscientificlab.Physics
{
    public class AABBShape: Shape
    {
        public static ObjectPool<AABBShape> Pool = new GenericObjectPool<AABBShape>(32);
        public Fix Width;
        public Fix Height;

        public override ShapeType Type
        {
            get { return ShapeType.AABB; }
        }

        public static AABBShape New(Fix width, Fix height)
        {
            var shape = Pool.Get();
            shape.Width = width;
            shape.Height = height;
            return shape;
        }

        public override void Return()
        {
            Pool.Return(this);
        }

        public override AABB GetBoundingBox(ref Transform transform)
        {
            return new AABB(transform.Position, Width / 2, Height / 2);
        }

    }
}