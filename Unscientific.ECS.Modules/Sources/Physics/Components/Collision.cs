using Unscientific.ECS.Modules.Core;
using Unscientific.ECS.Modules.Physics.Shapes;
using Unscientific.Util.Pool;

namespace Unscientific.ECS.Modules.Physics
{
    public class Collision
    {
        private static readonly ObjectPool<Collision> Pool = new GenericObjectPool<Collision>(512);

        public Shape SelfShape;
        public Entity<Game> Other;
        public Shape OtherShape;

        public static Collision New(Shape selfShape, Entity<Game> other, Shape otherShape)
        {
            var collision = Pool.Get();
            collision.SelfShape = selfShape;
            collision.Other = other;
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