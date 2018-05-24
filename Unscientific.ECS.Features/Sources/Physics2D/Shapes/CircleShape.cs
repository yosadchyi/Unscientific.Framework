using System;
using Unscientific.FixedPoint;
using Unscientific.Util.Pool;

namespace Unscientific.ECS.Features.Physics2D.Shapes
{
    public class CircleShape: Shape
    {
        private static readonly GenericObjectPool<CircleShape> Pool = new GenericObjectPool<CircleShape>(16);

        public Fix Radius;

        public static CircleShape New(Fix radius)
        {
            var instance = Pool.Get();
            instance.Radius = radius;
            return instance;
        }

        public static CircleShape New(Action<CircleShape> initialize)
        {
            var instance = Pool.Get();

            initialize(instance);
            return instance;
        }

        public CircleShape()
        {
            Type = ShapeType.Circle;
        }

        public override AABB GetBoundingBox(ref Transform transform)
        {
            return new AABB(transform.Position, Radius);
        }

        public override void ReturnToPool()
        {
            Clear();
            Radius = 0;
            Pool.Return(this);
        }
    }
}