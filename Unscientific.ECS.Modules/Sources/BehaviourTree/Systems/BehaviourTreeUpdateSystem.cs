using Unscientific.ECS.Modules.Core;

namespace Unscientific.ECS.Modules.BehaviourTree
{
    public class BehaviourTreeUpdateSystem: IUpdateSystem
    {
        private readonly Context<Game> _simulation;

        public BehaviourTreeUpdateSystem(Contexts contexts)
        {
            _simulation = contexts.Get<Game>();
        }

        public void Update()
        {
            foreach (var entity in _simulation.AllWith<BehaviourTreeData>())
            {
                if (entity.Is<Destroyed>())
                    continue;

                entity.Get<BehaviourTreeData>().BehaviourTree.Execute(entity);
            }
        }
    }
}