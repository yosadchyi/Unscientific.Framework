using Unscientificlab.ECS.Modules.Base;
using Unscientificlab.ECS.Modules.Physics;

namespace Unscientificlab.ECS.Modules.Steering
{
    public class SteeringSystem: IUpdateSystem
    {
        private readonly Context<Simulation> _simulation;

        public SteeringSystem(Contexts contexts)
        {
            _simulation = contexts.Get<Simulation>();
        }

        public void Update()
        {
            foreach (var entity in _simulation.AllWith<Steering>())
            {
                if (entity.Is<Destroyed>())
                    continue;

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
}