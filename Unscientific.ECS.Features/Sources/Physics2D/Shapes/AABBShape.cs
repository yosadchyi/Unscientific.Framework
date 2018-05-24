using System;
using Unscientific.FixedPoint;
using Unscientific.Util.Pool;

namespace Unscientific.ECS.Features.Physics2D.Shapes
{
    public class AABBShape: Shape
    {
        private static readonly GenericObjectPool<AABBShape> Pool = new GenericObjectPool<AABBShape>(16);

        public Fix Width;
        public Fix Height;

        public static AABBShape New(Fix width, Fix height)
        {
            var instance = Pool.Get();

            instance.Width = width;
            instance.Height = height;
            return instance;
        }

        public static AABBShape New(Action<AABBShape> initialize)
        {
            var instance = Pool.Get();

            initialize(instance);
            return instance;
        }

        public AABBShape()
        {
            Type = ShapeType.AABB;
        }

        public override AABB GetBoundingBox(ref Transform transform)
        {
            return new AABB(transform.Position, Width / 2, Height / 2);
        }

        public override void ReturnToPool()
        {
            Clear();
            Width = 0;
            Height = 0;
            Pool.Return(this);
        }
    }
}