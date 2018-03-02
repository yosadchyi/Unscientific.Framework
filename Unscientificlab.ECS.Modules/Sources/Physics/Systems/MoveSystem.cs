using Unscientificlab.ECS.Modules.Base;

namespace Unscientificlab.ECS.Modules.Physics
{
    public class MoveSystem: IUpdateSystem
    {
        private readonly Entity<Configuration> _configuration;
        private readonly Context<Simulation> _simulation;

        public MoveSystem(Contexts contexts)
        {
            _configuration = contexts.Get<Configuration>().First();
            _simulation = contexts.Get<Simulation>();
        }

        public void Update()
        {
            var dt = _configuration.Get<TimeStep>().Value;

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