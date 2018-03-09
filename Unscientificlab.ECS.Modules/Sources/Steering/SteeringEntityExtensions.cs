using Unscientificlab.ECS;
using Unscientificlab.ECS.Modules.Core;
using Unscientificlab.ECS.Modules.Physics;
using Unscientificlab.FixedPoint;
using Unscientificlab.ECS.Modules.Steering;

namespace Framework.Unscientificlab
{
    public static class SteeringEntityExtensions
    {
        public static bool TryGetTargetPosition(this Entity<Simulation> entity, Context<Simulation> simulation, ref FixVec2 output)
        {
            if (entity.Has<TargetEntity>())
            {
                var target = entity.Get<TargetEntity>().Reference.Entity;

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