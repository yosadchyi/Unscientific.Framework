using Unscientific.ECS.Modules.Core;

namespace Unscientific.ECS.Modules.BehaviourTree
{
    public class BehaviourTreeCleanupSystem: EntityCleanupSystem<Game, Destroyed, BehaviourTreeData>
    {
        public BehaviourTreeCleanupSystem(Contexts contexts) : base(contexts)
        {
        }

        protected override void Cleanup(Entity<Game> entity)
        {
            var data = entity.Get<BehaviourTreeData>().ExecutionData;

            data.Return();
            entity.Remove<BehaviourTreeData>();
        }
    }
}