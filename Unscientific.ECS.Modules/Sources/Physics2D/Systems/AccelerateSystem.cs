using Unscientific.ECS.Features.Core;
using Unscientific.ECS.Features.Destroy;
using Unscientific.FixedPoint;

namespace Unscientific.ECS.Features.Physics2D
{
    public class AccelerateSystem
    {
        public static void Update(Contexts contexts)
        {
            var dt = contexts.Configuration().Get<TimeStep>().Value;
            var g = contexts.Configuration().Get<GlobalForce>().Value;
            var context = contexts.Get<Game>();

            foreach (var entity in context.AllWith<Force, Mass, Velocity>())
            {
                if (entity.Is<Destroyed>())
                    continue;

                var force = entity.Get<Force>().Value + g;
                var mass = entity.Get<Mass>().Value;
                var velocity = entity.Get<Velocity>().Value;
                var damping = entity.Has<Damping>() ? entity.Get<Damping>().Value : 0;

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