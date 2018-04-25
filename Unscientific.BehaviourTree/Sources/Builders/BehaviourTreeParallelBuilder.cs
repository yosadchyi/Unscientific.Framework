namespace Unscientific.BehaviourTree
{
    public class BehaviourTreeParallelBuilder<TBlackboard, TParent> :
        BehaviourTreeBuilderBase<TBlackboard, BehaviourTreeParallelBuilder<TBlackboard, TParent>,
            BehaviourTreeParallelBuilder<TBlackboard, TParent>>,
        IBehaviourTreeEndableBuilder<TBlackboard, TParent>
        where TParent : INodeHandler<TBlackboard>
    {
        private readonly ParallelNode<TBlackboard> _group;
        private readonly TParent _parent;

        public BehaviourTreeParallelBuilder(TParent parent, ParallelNode<TBlackboard> group)
        {
            _parent = parent;
            _group = group;
        }

        public BehaviourTreeParallelBuilder<TBlackboard, TParent> WithFailurePolicy(ParallelPolicy failurePolicy)
        {
            _group.FailurePolicy = failurePolicy;
            return this;
        }

        public BehaviourTreeParallelBuilder<TBlackboard, TParent> WithSucceedPolicy(ParallelPolicy succeedPolicy)
        {
            _group.SucceedPolicy = succeedPolicy;
            return this;
        }

        protected override BehaviourTreeParallelBuilder<TBlackboard, TParent> HandleNode(
            BehaviourTreeNode<TBlackboard> node)
        {
            _group.AddChild(node);
            return this;
        }

        protected override BehaviourTreeParallelBuilder<TBlackboard, TParent> GetThisAsParentFor(
            BehaviourTreeNode<TBlackboard> node)
        {
            return this;
        }

        public TParent End()
        {
            _parent.DoHandleNode(_group);
            return _parent;
        }
    }
}