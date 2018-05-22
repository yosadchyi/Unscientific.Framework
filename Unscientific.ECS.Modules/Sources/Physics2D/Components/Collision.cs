using Unscientific.ECS.Features.Core;
using Unscientific.ECS.Features.Physics2D.Shapes;

namespace Unscientific.ECS.Features.Physics2D
{
    public struct Collision
    {
        public readonly Shape SelfShape;
        public readonly Entity<Game> Other;
        public readonly Shape OtherShape;

        public Collision(Shape selfShape, Entity<Game> other, Shape otherShape)
        {
            SelfShape = selfShape;
            Other = other;
            OtherShape = otherShape;
        }
    }
}