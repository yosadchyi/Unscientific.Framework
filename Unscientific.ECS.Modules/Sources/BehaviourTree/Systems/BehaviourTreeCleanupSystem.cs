using Unscientific.ECS.Modules.Core;

namespace Unscientific.ECS.Modules.BehaviourTree
{
    public class BehaviourTreeCleanupSystem<TScope>: EntityCleanupSystem<Game, Destroyed, BehaviourTreeData<TScope>>
        where TScope : IScope
    {
        public BehaviourTreeCleanupSystem(Contexts contexts) : base(contexts)
        {
        }

        protected override void Cleanup(Entity<Game> entity)
        {
            var data = entity.Get<BehaviourTreeData<TScope>>().ExecutionData;

            data.Return();
            entity.Remove<BehaviourTreeData<TScope>>();
        }
    }
}