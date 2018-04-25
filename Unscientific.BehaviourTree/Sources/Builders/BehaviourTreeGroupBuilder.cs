namespace Unscientific.BehaviourTree
{
    public class BehaviourTreeGroupBuilder<TBlackboard, TParent> :
        BehaviourTreeBuilderBase<TBlackboard, BehaviourTreeGroupBuilder<TBlackboard, TParent>,
            BehaviourTreeGroupBuilder<TBlackboard, TParent>>,
        IBehaviourTreeEndableBuilder<TBlackboard, TParent>
        where TParent : INodeAcceptor<TBlackboard>
    {
        private readonly CompositeNode<TBlackboard> _group;
        private readonly TParent _parent;

        public BehaviourTreeGroupBuilder(TParent parent, CompositeNode<TBlackboard> group)
        {
            _parent = parent;
            _group = group;
        }

        public override BehaviourTreeNode<TBlackboard> AcceptNode(BehaviourTreeNode<TBlackboard> node)
        {
            _group.AddChild(node);
            return node;
        }

        protected override BehaviourTreeGroupBuilder<TBlackboard, TParent> ConvertNodeToResult(BehaviourTreeNode<TBlackboard> node)
        {
            return this;
        }

        protected override BehaviourTreeGroupBuilder<TBlackboard, TParent> GetParentForNode(BehaviourTreeNode<TBlackboard> node)
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