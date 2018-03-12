using Unscientific.BehaviourTree;
using Unscientific.ECS.Modules.Core;

namespace Unscientific.ECS.Modules.BehaviourTree
{
    public struct BehaviourTreeData
    {
        public readonly BehaviourTree BehaviourTree;
        public readonly BehaviourTreeExecutionData<Entity<Simulation>> ExecutionData;

            public BehaviourTreeData(BehaviourTree behaviourTree, BehaviourTreeExecutionData<Entity<Simulation>> executionData = null)
            {
                BehaviourTree = behaviourTree;
                ExecutionData = executionData;
            }
    }
}
