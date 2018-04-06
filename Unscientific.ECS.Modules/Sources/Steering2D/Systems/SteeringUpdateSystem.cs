using Unscientific.ECS.Modules.Core;
using Unscientific.ECS.Modules.Physics2D;

namespace Unscientific.ECS.Modules.Steering2D
{
    public class SteeringUpdateSystem: IUpdateSystem
    {
        private readonly Context<Game> _game;
        private readonly Context<Singletons> _singletons;
        private readonly Context<Configuration> _configuration;

        public SteeringUpdateSystem(Contexts contexts)
        {
            _game = contexts.Get<Game>();
            _singletons = contexts.Get<Singletons>();
            _configuration = contexts.Get<Configuration>();
        }

        public void Update()
        {
            var updatePeriod = _configuration.Singleton().Get<SteeringUpdatePeriod>().PeriodInTicks;
            var tick = _singletons.Singleton().Get<Tick>().Value;

            if (tick % updatePeriod != 0) return;

            foreach (var entity in _game.AllWith<Steering>())
            {
                if (entity.Is<Destroyed>()) continue;

                var steeringVelocity = SteeringVelocity.Zero;
                var steeringBehaviour = entity.Get<Steering>().SteeringBehaviour;
                var velocity = steeringBehaviour.Calculate(entity, ref steeringVelocity);

                if (entity.Has<Velocity>()) entity.Replace(new Velocity(velocity.Linear));
                if (entity.Has<AngularVelocity>()) entity.Replace(new AngularVelocity(velocity.Angular));
            }
        }
    }
}