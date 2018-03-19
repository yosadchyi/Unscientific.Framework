using Unscientific.ECS.Modules.Core;
using Unscientific.FixedPoint;

namespace Unscientific.ECS.Modules.Physics
{
    public class SetupSystem: ISetupSystem
    {
        private readonly Entity<Configuration> _singleton;
        private readonly ISpatialDatabase _spatialDatabase;
        private readonly Fix _timeStep;

        public SetupSystem(Contexts contexts, ISpatialDatabase spatialDatabase, Fix timeStep)
        {
            _singleton = contexts.Singleton<Configuration>();
            _spatialDatabase = spatialDatabase;
            _timeStep = timeStep;
        }

        public void Setup()
        {
            _singleton
                .Add(new Space(_spatialDatabase))
                .Add(new TimeStep(_timeStep));
        }
    }
}