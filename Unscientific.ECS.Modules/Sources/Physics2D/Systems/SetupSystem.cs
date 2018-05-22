using Unscientific.ECS.Features.Core;
using Unscientific.FixedPoint;

namespace Unscientific.ECS.Features.Physics2D
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