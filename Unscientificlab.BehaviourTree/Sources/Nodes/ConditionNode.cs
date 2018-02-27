using System;

namespace Unscientificlab.BehaviourTree
{
    public class ConditionNode<TBlackboard> : BehaviourTreeNode<TBlackboard>
    {
        private readonly Func<TBlackboard, bool> _predicate;

        public ConditionNode(string name, Func<TBlackboard, bool> predicate) : base(name)
        {
            _predicate = predicate;
        }

        protected override BehaviourTreeStatus Update(BehaviourTreeExecutor<TBlackboard> executor, BehaviourTreeExecutionData<TBlackboard> data, TBlackboard blackboard)
        {
            return _predicate(blackboard) ? BehaviourTreeStatus.Success : BehaviourTreeStatus.Failure;
        }
    }
}