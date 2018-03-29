using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Unscientific.ECS.Modules.Core;
using Unscientific.ECS.Modules.Physics.Shapes;
using Unscientific.FixedPoint;

namespace Unscientific.ECS.Modules.Physics
{
    public class SpatialDatanaseCleanupSystem: ICleanupSystem
    {
        private readonly Context<Singletons> _singletons;

        [SuppressMessage("ReSharper", "HeapView.DelegateAllocation")]
        public SpatialDatanaseCleanupSystem(Contexts contexts)
        {
            _singletons = contexts.Get<Singletons>();
        }

        public void Cleanup()
        {
            _singletons.Singleton().Get<Space>().SpatialDatabase.Clear();
        }
    }
}