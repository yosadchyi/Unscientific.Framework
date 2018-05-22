﻿using Unscientific.BehaviourTree;
using Unscientific.ECS.Modules.Core;
using Unscientific.ECS.Modules.Tick;

namespace Unscientific.ECS.Modules.BehaviourTree
{
    public class TickSupplier: ITickSupplier
    {
        private readonly Context<Singletons> _context;

        public TickSupplier(Contexts contexts)
        {
            _context = contexts.Get<Singletons>();
        }

        public int Supply()
        {
            return _context.Singleton().Get<TickCounter>().Value;
        }
    }
}