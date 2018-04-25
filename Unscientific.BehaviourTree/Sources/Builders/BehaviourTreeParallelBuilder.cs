namespace Unscientific.BehaviourTree
{
    public class BehaviourTreeParallelBuilder<TBlackboard, TParent> :
        BehaviourTreeBuilderBase<TBlackboard, BehaviourTreeParallelBuilder<TBlackboard, TParent>,
            BehaviourTreeParallelBuilder<TBlackboard, TParent>>,
        IBehaviourTreeEndableBuilder<TBlackboard, TParent>
        where TParent : INodeAcceptor<TBlackboard>
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

        public override BehaviourTreeNode<TBlackboard> AcceptNode(BehaviourTreeNode<TBlackboard> node)
        {
            _group.AddChild(node);
            return node;
        }

        protected override BehaviourTreeParallelBuilder<TBlackboard, TParent> ConvertNodeToResult(
            BehaviourTreeNode<TBlackboard> node)
        {
            return this;
        }

        protected override BehaviourTreeParallelBuilder<TBlackboard, TParent> GetParentForNode(
            BehaviourTreeNode<TBlackboard> node)
        {
            return this;
        }

        public TParent End()
        {
            _parent.AcceptNode(_group);
            return _parent;
        }
    }
}