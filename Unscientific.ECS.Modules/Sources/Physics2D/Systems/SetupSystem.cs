using Unscientific.ECS.Modules.Core;
using Unscientific.FixedPoint;

namespace Unscientific.ECS.Modules.Physics2D
{
    public static class SetupSystem
    {
        public static void Setup(Contexts contexts, Fix timeStep, ISpatialDatabase spatialDatabase)
        {
            contexts.Singleton()
                .Add(new Space(spatialDatabase));
            contexts.Configuration()
                .Add(new TimeStep(timeStep))
                .Add(new GlobalForce(FixVec2.Zero));
        }
    }
}