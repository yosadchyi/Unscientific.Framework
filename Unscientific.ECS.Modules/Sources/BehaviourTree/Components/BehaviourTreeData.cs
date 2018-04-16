using Unscientific.BehaviourTree;
using Unscientific.ECS.Modules.Core;

namespace Unscientific.ECS.Modules.BehaviourTree
{
    public struct BehaviourTreeData<TScope> where TScope : IScope
    {
        public readonly BehaviourTree<TScope> BehaviourTree;
        public readonly BehaviourTreeExecutionData<Entity<TScope>> ExecutionData;

        public BehaviourTreeData(BehaviourTree<TScope> behaviourTree, BehaviourTreeExecutionData<Entity<TScope>> executionData = null)
        {
            BehaviourTree = behaviourTree;
            ExecutionData = executionData;
        }
    }
}
