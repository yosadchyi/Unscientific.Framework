using Unscientific.ECS.Features.Core;
using Unscientific.ECS.Features.Destroy;
using Unscientific.FixedPoint;

namespace Unscientific.ECS.Features.Physics2D
{
    public static class AngularAccelerateSystem
    {
        public static void Update(Contexts contexts)
        {
            var dt = contexts.Configuration().Get<TimeStep>().Value;
            var context = contexts.Get<Game>();

            foreach (var entity in context.AllWith<Torque, Inertia, AngularVelocity>())
            {
                if (entity.Is<Destroyed>())
                    continue;

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