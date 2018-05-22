using Unscientific.BehaviourTree;
using Unscientific.ECS.Features.Core;
using Unscientific.ECS.Features.Tick;

namespace Unscientific.ECS.Features.BehaviourTree
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