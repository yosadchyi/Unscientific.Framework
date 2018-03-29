using Unscientific.ECS.Modules.Core;
using Unscientific.FixedPoint;

namespace Unscientific.ECS.Modules.Physics2D
{
    public class SetupSystem: ISetupSystem
    {
        private readonly Context<Configuration> _configuration;
        private readonly Context<Singletons> _singletons;
        private readonly ISpatialDatabase _spatialDatabase;
        private readonly Fix _timeStep;

        public SetupSystem(Contexts contexts, ISpatialDatabase spatialDatabase, Fix timeStep)
        {
            _configuration = contexts.Get<Configuration>();
            _singletons = contexts.Get<Singletons>();
            _spatialDatabase = spatialDatabase;
            _timeStep = timeStep;
        }

        public void Setup()
        {
            _singletons.Singleton()
                .Add(new Space(_spatialDatabase));
            _configuration.Singleton()
                .Add(new TimeStep(_timeStep))
                .Add(new GlobalForce(FixVec2.Zero));
        }
    }
}