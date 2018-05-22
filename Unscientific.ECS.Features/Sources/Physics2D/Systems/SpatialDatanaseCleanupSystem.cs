using Unscientific.ECS.Features.Core;

namespace Unscientific.ECS.Features.Physics2D
{
    public static class SpatialDatanaseCleanupSystem
    {
        public static void Cleanup(Contexts contexts)
        {
            contexts.Singleton().Get<Space>().SpatialDatabase.Clear();
        }
    }
}