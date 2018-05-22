using Unscientific.ECS.Modules.Core;
using Unscientific.ECS.Modules.Destroy;

namespace Unscientific.ECS.Modules.Physics2D
{
    public static class MoveSystem
    {
        public static void Update(Contexts contexts)
        {
            var dt = contexts.Singleton().Get<TimeStep>().Value;
            var context = contexts.Get<Game>();

            foreach (var entity in context.AllWith<Position, Velocity>())
            {
                if (entity.Is<Destroyed>())
                    continue;

                var position = entity.Get<Position>();
                var velocity = entity.Get<Velocity>();

                entity.Replace(new Position(position.Value + velocity.Value * dt));
            }
        }
    }
}