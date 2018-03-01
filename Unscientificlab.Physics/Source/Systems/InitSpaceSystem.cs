using Unscientificlab.ECS;
using Unscientificlab.ECS.Base;

namespace Unscientificlab.Physics
{
    public class InitSpaceSystem: ISetupSystem
    {
        private readonly ISpatialDatabase _spatialDatabase;
        private readonly Context<Singletons> _singletons;

        public InitSpaceSystem(Contexts contexts, ISpatialDatabase spatialDatabase)
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