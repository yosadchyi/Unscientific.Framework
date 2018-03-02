using Unscientificlab.ECS.Modules.Base;

namespace Unscientificlab.ECS.Modules.Physics
{
    public class SpaceSetupSystem: ISetupSystem
    {
        private readonly ISpatialDatabase _spatialDatabase;
        private readonly Context<Singletons> _singletons;

        public SpaceSetupSystem(Contexts contexts, ISpatialDatabase spatialDatabase)
        {
            _spatialDatabase = spatialDatabase;
            _singletons = contexts.Get<Singletons>();
        }

        public void Setup()
        {
            _singletons.First().Add(new Space(_spatialDatabase));
        }
    }
}