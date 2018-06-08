using System.Runtime.Serialization.Formatters;
using Unscientific.ECS.Features.Core;
using Unscientific.ECS.Features.Destroy;
using Unscientific.ECS.Features.Physics2D;
using Unscientific.FixedPoint;

namespace Unscientific.ECS.Features.Steering2D
{
    public static class SteeringEntityExtensions
    {
        public static bool TryGetTargetPosition(this Entity<Game> entity, ref FixVec2 output)
        {
            if (entity.Has<TargetEntity>())
            {
                var target = entity.Get<TargetEntity>().Entity;

                if (target.Alive && !target.Is<Destroyed>())
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