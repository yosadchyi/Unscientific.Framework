using Unscientific.ECS.Modules.Core;

namespace Unscientific.ECS.Modules.Physics2D
{
    public class MoveSystem: IUpdateSystem
    {
        private readonly Context<Configuration> _configuration;
        private readonly Context<Game> _context;

        public MoveSystem(Contexts contexts)
        {
            _configuration = contexts.Get<Configuration>();
            _context = contexts.Get<Game>();
        }

        public void Update()
        {
            var dt = _configuration.Singleton().Get<TimeStep>().Value;

            foreach (var entity in _context.AllWith<Position, Velocity>())
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