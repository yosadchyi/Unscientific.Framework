namespace Unscientific.BehaviourTree
{
    public class SelectorNode<TBlackboard> : CompositeNode<TBlackboard>, ICompletionObserver<TBlackboard>
    {
        private int _currentChildId;

        public SelectorNode(string name) : base(name)
        {
        }

        public override void DeclareNode(BehaviourTreeMetadata<TBlackboard> metadata)
        {
            _currentChildId = metadata.DeclareVariable(this, "currentChild");
            base.DeclareNode(metadata);
        }

        private BehaviourTreeNode<TBlackboard> GetCurrentChild(BehaviourTreeExecutionData<TBlackboard> data)
        {
            return Children[data.Variables[_currentChildId]];
        }

        public override void Initialize(BehaviourTreeExecutor<TBlackboard> executor, BehaviourTreeExecutionData<TBlackboard> data, TBlackboard blackboard)
        {
            data.Variables[_currentChildId] = 0;
            executor.Start(data, GetCurrentChild(data), this);
        }

        public void OnComplete(BehaviourTreeExecutor<TBlackboard> executor, BehaviourTreeExecutionData<TBlackboard> execution, BehaviourTreeStatus status)
        {
            var node = GetCurrentChild(execution);

            if (execution.Statuses[node.Id] == BehaviourTreeStatus.Success)
            {
                executor.Stop(execution, this, BehaviourTreeStatus.Success);
                return;
            }

            var currentChild = ++execution.Variables[_currentChildId];

            if (currentChild == ChildrenCount)
            {
                executor.Stop(execution, this, BehaviourTreeStatus.Failure);
            }
            else
            {
                executor.Start(execution, GetCurrentChild(execution), this);
            }
        }

        protected override BehaviourTreeStatus Update(BehaviourTreeExecutor<TBlackboard> executor, BehaviourTreeExecutionData<TBlackboard> data, TBlackboard blackboard)
        {
            return BehaviourTreeStatus.Running;
        }
    }
}