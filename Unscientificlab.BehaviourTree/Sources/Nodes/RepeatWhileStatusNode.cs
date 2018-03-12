namespace Unscientific.BehaviourTree
{
    public class RepeatWhileStatus<TBlackboard> : BaseDecoratorNode<TBlackboard>, ICompletionObserver<TBlackboard>
    {
        private readonly BehaviourTreeStatus _status;

        public RepeatWhileStatus(string name, BehaviourTreeStatus status) : base(name)
        {
            _status = status;
        }

        public override void Initialize(BehaviourTreeExecutor<TBlackboard> executor, BehaviourTreeExecutionData<TBlackboard> data, TBlackboard blackboard)
        {
            executor.Start(data, Node, this);
        }

        public void OnComplete(BehaviourTreeExecutor<TBlackboard> executor, BehaviourTreeExecutionData<TBlackboard> execution, BehaviourTreeStatus status)
        {
            if (status != _status)
            {
                executor.Stop(execution, this, BehaviourTreeStatus.Success);
            }
            else
            {
                executor.Schedule(execution, Node, this);
            }
        }

        protected override BehaviourTreeStatus Update(BehaviourTreeExecutor<TBlackboard> executor,
            BehaviourTreeExecutionData<TBlackboard> data,
            TBlackboard blackboard)
        {
            return BehaviourTreeStatus.Running;
        }
    }
}