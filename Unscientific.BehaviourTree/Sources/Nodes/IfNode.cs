using System;

namespace Unscientific.BehaviourTree
{
    public class IfNode<TBlackboard> : BaseDecoratorNode<TBlackboard>, ICompletionObserver<TBlackboard>
    {
        private readonly Func<TBlackboard, bool> _predicate;

        public IfNode(string name, Func<TBlackboard, bool> predicate) : base(name)
        {
            _predicate = predicate;
        }

        public override void Initialize(BehaviourTreeExecutor<TBlackboard> executor, BehaviourTreeExecutionData<TBlackboard> data, TBlackboard blackboard)
        {
            if (_predicate(blackboard))
            {
                executor.Start(data, Node, this);
            }
        }

        protected override BehaviourTreeStatus Update(BehaviourTreeExecutor<TBlackboard> executor,
            BehaviourTreeExecutionData<TBlackboard> data,
            TBlackboard blackboard)
        {
            return BehaviourTreeStatus.Failure;
        }

        public void OnComplete(BehaviourTreeExecutor<TBlackboard> executor,
            BehaviourTreeExecutionData<TBlackboard> execution,
            BehaviourTreeStatus status)
        {
            executor.Stop(execution, this, status);
        }
    }
}