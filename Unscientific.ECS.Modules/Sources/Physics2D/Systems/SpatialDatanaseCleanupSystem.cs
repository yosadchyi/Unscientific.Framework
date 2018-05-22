using System.Diagnostics.CodeAnalysis;
using Unscientific.ECS.Modules.Core;

namespace Unscientific.ECS.Modules.Physics2D
{
    public static class SpatialDatanaseCleanupSystem
    {
        public static void Cleanup(Contexts contexts)
        {
            contexts.Singleton().Get<Space>().SpatialDatabase.Clear();
        }
    }
}