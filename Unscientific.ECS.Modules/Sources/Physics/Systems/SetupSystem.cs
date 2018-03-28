using Unscientific.ECS.Modules.Core;
using Unscientific.FixedPoint;

namespace Unscientific.ECS.Modules.Physics
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
            ContextExtensions.Singleton(_singletons)
                .Add(new Space(_spatialDatabase));
            ContextExtensions.Singleton(_configuration)
                .Add(new TimeStep(_timeStep));
        }
    }
}