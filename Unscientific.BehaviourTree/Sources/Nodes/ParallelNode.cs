namespace Unscientific.BehaviourTree
{
    public enum ParallelPolicy
    {
        RequireOne,
        RequireAll
    }

    public class ParallelNode<TBlackboard> : CompositeNode<TBlackboard>, ICompletionObserver<TBlackboard>
    {
        public ParallelPolicy FailurePolicy { get; set; }

        public ParallelPolicy SucceedPolicy { get; set; }

        private int _succeededCountId;
        private int _failedCountId;

        public ParallelNode(string name) : base(name)
        {
            FailurePolicy = ParallelPolicy.RequireOne;
            SucceedPolicy = ParallelPolicy.RequireAll;
        }

        public override void DeclareNode(BehaviourTreeMetadata<TBlackboard> metadata)
        {
            _succeededCountId = metadata.DeclareVariable(this, "succeededCountId");
            _failedCountId = metadata.DeclareVariable(this, "failedCountId");
            base.DeclareNode(metadata);
        }

        public override void Initialize(BehaviourTreeExecutor<TBlackboard> executor, BehaviourTreeExecutionData<TBlackboard> data, TBlackboard blackboard)
        {
            data.Variables[_failedCountId] = 0;
            data.Variables[_succeededCountId] = 0;

            foreach (var node in Children)
                executor.Start(data, node, this);
        }

        public void OnComplete(BehaviourTreeExecutor<TBlackboard> executor, BehaviourTreeExecutionData<TBlackboard> execution, BehaviourTreeStatus status)
        {
            if (status == BehaviourTreeStatus.Success)
            {
                ++execution.Variables[_succeededCountId];
                if (SucceedPolicy == ParallelPolicy.RequireOne)
                {
                    Stop(executor, execution, BehaviourTreeStatus.Success);
                    return;
                }
            }
            else if (status == BehaviourTreeStatus.Failure)
            {
                ++execution.Variables[_failedCountId];
                if (FailurePolicy == ParallelPolicy.RequireOne)
                {
                    Stop(executor, execution, BehaviourTreeStatus.Failure);
                    return;
                }
            }

            if (FailurePolicy == ParallelPolicy.RequireAll && execution.Variables[_failedCountId] == ChildrenCount)
            {
                Stop(executor, execution, BehaviourTreeStatus.Failure);
            }
            else if (SucceedPolicy == ParallelPolicy.RequireAll && execution.Variables[_succeededCountId] == ChildrenCount)
            {
                Stop(executor, execution, BehaviourTreeStatus.Success);
            }
        }

        private void Stop(BehaviourTreeExecutor<TBlackboard> executor,
            BehaviourTreeExecutionData<TBlackboard> data,
            BehaviourTreeStatus status)
        {
            AbortRunningChildren(executor, data);
            executor.Stop(data, this, status);
        }

        protected override BehaviourTreeStatus Update(BehaviourTreeExecutor<TBlackboard> executor, BehaviourTreeExecutionData<TBlackboard> data, TBlackboard blackboard)
        {
            return BehaviourTreeStatus.Running;
        }

        public void AbortRunningChildren(BehaviourTreeExecutor<TBlackboard> executor,
            BehaviourTreeExecutionData<TBlackboard> data)
        {
            foreach (var node in Children)
            {
                if (node.IsRunning(data))
                    executor.Abort(data, node);
            }
        }
    }
}