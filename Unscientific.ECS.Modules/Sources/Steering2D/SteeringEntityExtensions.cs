using Unscientific.ECS.Modules.Core;
using Unscientific.ECS.Modules.Physics2D;
using Unscientific.FixedPoint;

namespace Unscientific.ECS.Modules.Steering2D
{
    public static class SteeringEntityExtensions
    {
        public static bool TryGetTargetPosition(this Entity<Game> entity, ref FixVec2 output)
        {
            if (entity.Has<TargetEntity>())
            {
                var target = entity.Get<TargetEntity>().Entity;

                if (!target.Is<Destroyed>())
                {
                    output = target.Get<Position>().Value;
                    return true;
                }
            }
            if (entity.Has<TargetPosition>())
            {
                output = entity.Get<TargetPosition>().Value;
                return true;
            }
            return false;
        }
    }
}