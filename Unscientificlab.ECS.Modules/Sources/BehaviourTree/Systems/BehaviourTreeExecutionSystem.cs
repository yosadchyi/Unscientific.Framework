using Unscientificlab.ECS.Modules.Base;

namespace Unscientificlab.ECS.Modules.BehaviourTree
{
    public class BehaviourTreeExecutionSystem: IUpdateSystem
    {
        private readonly Context<Simulation> _simulation;

        public BehaviourTreeExecutionSystem(Contexts contexts)
        {
            _simulation = contexts.Get<Simulation>();
        }

        public void Update()
        {
            foreach (var entity in _simulation.AllWith<BehaviourTreeData>())
            {
                entity.Get<BehaviourTreeData>().BehaviourTree.Execute(entity);
            }
        }
    }
}