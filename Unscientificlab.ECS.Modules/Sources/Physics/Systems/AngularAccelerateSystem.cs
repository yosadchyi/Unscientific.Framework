using Unscientificlab.ECS.Modules.Base;
using Unscientificlab.FixedPoint;

namespace Unscientificlab.ECS.Modules.Physics
{
    public class AngularAccelerateSystem: IUpdateSystem
    {
        private readonly Entity<Configuration> _configuration;
        private readonly Context<Simulation> _simulation;

        public AngularAccelerateSystem(Contexts contexts)
        {
            _simulation = contexts.Get<Simulation>();
            _configuration = contexts.Get<Configuration>().First();
        }

        public void Update()
        {
            var dt = _configuration.Get<TimeStep>().Value;

            foreach (var entity in _simulation.AllWith<AngularVelocity, Torque, Inertia>())
            {
                var velocity = entity.Get<AngularVelocity>().Value;
                var damping = entity.Has<AngularDamping>() ? entity.Get<AngularDamping>().Value : 0;
                var torque = entity.Get<Torque>().Value;
                var inertia = entity.Get<Inertia>().Value;

                velocity += dt * torque / inertia;
                velocity *= Fix.One / (Fix.One + dt * damping);

                if (entity.Has<MaxAngularVelocity>())
                {
                    var maxVelocity = entity.Get<MaxAngularVelocity>();

                    if (velocity > maxVelocity.Value)
                        velocity = maxVelocity.Value;
                }

                entity.Replace(new AngularVelocity(velocity));
                entity.Replace(new Torque());
            }
        }
    }
}