using Unscientific.ECS.Modules.Core;

namespace Unscientific.ECS.Modules.BehaviourTree
{
    public class BehaviourTreeCleanupSystem: ICleanupSystem
    {
        private readonly Context<Game> _context;

        public BehaviourTreeCleanupSystem(Contexts contexts)
        {
            _context = contexts.Get<Game>();
        }
        
        public void Cleanup()
        {
            foreach (var entity in _context.AllWith<Destroyed, BehaviourTreeData>())
            {
                var data = entity.Get<BehaviourTreeData>().ExecutionData;

                data.Return();
                entity.Remove<BehaviourTreeData>();
            }
        }
    }
}