using Unscientific.ECS.Modules.Core;

namespace Unscientific.ECS.Modules.Physics2D
{
    public class RotateSystem: IUpdateSystem
    {
        private readonly Context<Configuration> _configuration;
        private readonly Context<Game> _context;

        public RotateSystem(Contexts contexts)
        {
            _configuration = contexts.Get<Configuration>();
            _context = contexts.Get<Game>();
        }

        public void Update()
        {
            var dt = _configuration.Singleton().Get<TimeStep>().Value;

            foreach (var entity in _context.AllWith<Orientation, AngularVelocity>())
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