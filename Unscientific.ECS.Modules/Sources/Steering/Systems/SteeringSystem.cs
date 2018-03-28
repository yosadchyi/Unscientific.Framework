using Unscientific.ECS.Modules.Core;
using Unscientific.ECS.Modules.Physics;

namespace Unscientific.ECS.Modules.Steering
{
    public class SteeringSystem: EntityUpdateSystem<Game, Steering>
    {
        public SteeringSystem(Contexts contexts) : base(contexts)
        {
        }

        protected override void Update(Entity<Game> entity)
        {
            if (entity.Is<Destroyed>())
                return;

            var steeringVelocity = SteeringVelocity.Zero;
            var steeringBehaviour = entity.Get<Steering>().SteeringBehaviour;
            var velocity = steeringBehaviour.Calculate(entity, ref steeringVelocity);

            if (entity.Has<Velocity>())
                entity.Replace(new Velocity(velocity.Linear));
            if (entity.Has<AngularVelocity>())
                entity.Replace(new AngularVelocity(velocity.Angular));
        }
    }
}