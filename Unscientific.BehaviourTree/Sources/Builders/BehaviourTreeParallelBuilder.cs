namespace Unscientific.BehaviourTree
{
    public class BehaviourTreeParallelBuilder<TBlackboard, TFinalizeResult> :
        BehaviourTreeBuilderBase<TBlackboard, BehaviourTreeParallelBuilder<TBlackboard, TFinalizeResult>>
    {
        private readonly ParallelNode<TBlackboard> _group;
        private readonly TFinalizeResult _parent;

        public BehaviourTreeParallelBuilder(TFinalizeResult parent, ParallelNode<TBlackboard> group)
        {
            _parent = parent;
            _group = group;
        }

        public BehaviourTreeParallelBuilder<TBlackboard, TFinalizeResult> WithFailurePolicy(ParallelPolicy failurePolicy)
        {
            _group.FailurePolicy = failurePolicy;
            return this;
        }

        public BehaviourTreeParallelBuilder<TBlackboard, TFinalizeResult> WithSucceedPolicy(ParallelPolicy succeedPolicy)
        {
            _group.SucceedPolicy = succeedPolicy;
            return this;
        }

        protected override void AcceptNode(BehaviourTreeNode<TBlackboard> node)
        {
            _group.AddChild(node);
        }

        protected override BehaviourTreeParallelBuilder<TBlackboard, TFinalizeResult> GetBuilderMethodResult()
        {
            return this;
        }

        public TFinalizeResult End()
        {
            return _parent;
        }
    }
}