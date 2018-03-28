using Unscientific.ECS.Modules.Core;

namespace Unscientific.ECS.Modules.BehaviourTree
{
    public class BehaviourTreeUpdateSystem: EntityUpdateSystem<Game, BehaviourTreeData>
    {
        public BehaviourTreeUpdateSystem(Contexts contexts) : base(contexts)
        {
        }

        protected override void Update(Entity<Game> entity)
        {
            if (!entity.Is<Destroyed>())
                entity.Get<BehaviourTreeData>().BehaviourTree.Execute(entity);
        }
    }
}