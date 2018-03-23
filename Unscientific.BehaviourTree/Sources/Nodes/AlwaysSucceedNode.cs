namespace Unscientific.BehaviourTree
{
    public class SucceederNode<TBlackboard> : BaseDecoratorNode<TBlackboard>, ICompletionObserver<TBlackboard>
    {
        public SucceederNode(string name) : base(name)
        {
        }

        public override void Initialize(BehaviourTreeExecutor<TBlackboard> executor, BehaviourTreeExecutionData<TBlackboard> data, TBlackboard blackboard)
        {
            executor.Start(data, Node, this);
        }

        public void OnComplete(BehaviourTreeExecutor<TBlackboard> executor, BehaviourTreeExecutionData<TBlackboard> execution, BehaviourTreeStatus status)
        {
            executor.Stop(execution, this, BehaviourTreeStatus.Success);
        }

        protected override BehaviourTreeStatus Update(BehaviourTreeExecutor<TBlackboard> executor, BehaviourTreeExecutionData<TBlackboard> data, TBlackboard blackboard)
        {
            return BehaviourTreeStatus.Running;
        }
    }
}