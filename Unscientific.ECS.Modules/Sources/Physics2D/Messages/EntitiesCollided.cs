using Unscientific.ECS.Modules.Core;
using Unscientific.ECS.Modules.Physics2D.Shapes;

namespace Unscientific.ECS.Modules.Physics2D
{
    public struct EntitiesCollided
    {
        public readonly Entity<Game> EntityA;
        public readonly Shape ShapeA;
        public readonly Entity<Game> EntityB;
        public readonly Shape ShapeB;

        public EntitiesCollided(Entity<Game> entityA, Shape shapeA, Entity<Game> entityB, Shape shapeB)
        {
            EntityA = entityA;
            ShapeA = shapeA;
            EntityB = entityB;
            ShapeB = shapeB;
        }
    }
}