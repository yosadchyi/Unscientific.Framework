using Unscientific.ECS.Modules.Core;
using Unscientific.FixedPoint;

namespace Unscientific.ECS.Modules.Physics
{
    public class AccelerateSystem: IUpdateSystem
    {
        private readonly Context<Configuration> _configuration;
        private readonly Context<Game> _simulation;

        public AccelerateSystem(Contexts contexts)
        {
            _simulation = contexts.Get<Game>();
            _configuration = contexts.Get<Configuration>();
        }

        public void Update()
        {
            var dt = ContextExtensions.Singleton(_configuration).Get<TimeStep>().Value;

            foreach (var entity in _simulation.AllWith<Velocity, Force, Mass>())
            {
                if (entity.Is<Destroyed>())
                    continue;

                var velocity = entity.Get<Velocity>().Value;
                var damping = entity.Has<Damping>() ? entity.Get<Damping>().Value : 0;
                var force = entity.Get<Force>().Value;
                var mass = entity.Get<Mass>().Value;

                velocity += dt * force / mass;
                velocity *= Fix.One / (Fix.One + dt * damping);

                if (entity.Has<MaxVelocity>())
                {
                    var maxVelocity = entity.Get<MaxVelocity>();

                    velocity = FixVec2.ClampMagnitude (velocity, maxVelocity.Value);
                }

                entity.Replace(new Velocity(velocity));
                entity.Replace(new Force());
            }
        }
    }
}