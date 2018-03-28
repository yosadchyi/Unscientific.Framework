using Unscientific.ECS.Modules.Core;

namespace Unscientific.ECS.Modules.Physics
{
    public class MoveSystem: IUpdateSystem
    {
        private readonly Context<Configuration> _configuration;
        private readonly Context<Game> _simulation;

        public MoveSystem(Contexts contexts)
        {
            _configuration = contexts.Get<Configuration>();
            _simulation = contexts.Get<Game>();
        }

        public void Update()
        {
            var dt = ContextExtensions.Singleton(_configuration).Get<TimeStep>().Value;

            foreach (var entity in _simulation.AllWith<Position, Velocity>())
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