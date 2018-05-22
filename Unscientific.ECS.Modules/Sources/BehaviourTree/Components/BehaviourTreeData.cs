using Unscientific.BehaviourTree;

namespace Unscientific.ECS.Features.BehaviourTree
{
    public struct BehaviourTreeData<TScope>
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
