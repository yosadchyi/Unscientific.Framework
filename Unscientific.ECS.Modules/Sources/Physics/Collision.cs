using Unscientific.ECS.Modules.Core;
using Unscientific.ECS.Modules.Physics.Shapes;
using Unscientific.Util.Pool;

namespace Unscientific.ECS.Modules.Physics
{
    public struct EntitiesCollided
    {
        public readonly Entity<Game> EntityA;
        public readonly Shape ShapeA;
        public readonly Entity<Game> EntityB;
        public readonly Shape ShapeB;

        public EntitiesCollided(Entity<Game> entityA, Shape shapeA, Entity<Game> entityB, Shape shapeB)
        {
            this.EntityA = entityA;
            this.ShapeA = shapeA;
            this.EntityB = entityB;
            this.ShapeB = shapeB;
        }
    }

    public class Collision
    {
        public static ObjectPool<Collision> Pool = new GenericObjectPool<Collision>(512);

        public Shape SelfShape;
        public int Other;
        public Shape OtherShape;

        public static Collision New(Shape selfShape, Entity<Game> other, Shape otherShape)
        {
            var collision = Pool.Get();
            collision.SelfShape = selfShape;
            collision.Other = other.Id;
            collision.OtherShape = otherShape;
            return collision;
        }

        public void Return()
        {
            SelfShape = null;
            OtherShape = null;
            Pool.Return(this);
        }

    }
}