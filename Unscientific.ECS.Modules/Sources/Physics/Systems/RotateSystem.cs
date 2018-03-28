using Unscientific.ECS.Modules.Core;

namespace Unscientific.ECS.Modules.Physics
{
    public class RotateSystem: IUpdateSystem
    {
        private readonly Context<Configuration> _configuration;
        private readonly Context<Game> _simulation;

        public RotateSystem(Contexts contexts)
        {
            _configuration = contexts.Get<Configuration>();
            _simulation = contexts.Get<Game>();
        }

        public void Update()
        {
            var dt = ContextExtensions.Singleton(_configuration).Get<TimeStep>().Value;

            foreach (var entity in _simulation.AllWith<Orientation, AngularVelocity>())
            {
                if (entity.Is<Destroyed>())
                    continue;

                var orientation = entity.Get<Orientation>().Value;
                var angularVelocity = entity.Get<AngularVelocity>().Value;

                entity.Replace(new Orientation(orientation + angularVelocity * dt));
            }
        }
    }
}