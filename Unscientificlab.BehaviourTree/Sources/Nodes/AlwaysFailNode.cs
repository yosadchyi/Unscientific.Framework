namespace Unscientificlab.BehaviourTree
{
    public class FailerNode<TBlackboard> : BaseDecoratorNode<TBlackboard>, ICompletionObserver<TBlackboard>
    {
        public FailerNode(string name) : base(name)
        {
        }

        public override void Initialize(BehaviourTreeExecutor<TBlackboard> executor, BehaviourTreeExecutionData<TBlackboard> data, TBlackboard blackboard)
        {
            executor.Start(data, Node, this);
        }

        public void OnComplete(BehaviourTreeExecutor<TBlackboard> executor, BehaviourTreeExecutionData<TBlackboard> execution, BehaviourTreeStatus status)
        {
            executor.Stop(execution, this, BehaviourTreeStatus.Failure);
        }

        protected override BehaviourTreeStatus Update(BehaviourTreeExecutor<TBlackboard> executor,
            BehaviourTreeExecutionData<TBlackboard> data,
            TBlackboard blackboard)
        {
            return BehaviourTreeStatus.Running;
        }
    }
}