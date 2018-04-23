namespace Unscientific.BehaviourTree
{
    public class BehaviourTreeGroupBuilder<TBlackboard, TParent> :
        BehaviourTreeBuilderBase<TBlackboard, BehaviourTreeGroupBuilder<TBlackboard, TParent>,
            BehaviourTreeGroupBuilder<TBlackboard, TParent>>,
        IBehaviourTreeEndableBuilder<TBlackboard, TParent>
        where TParent : INodeHandler<TBlackboard>
    {
        private readonly CompositeNode<TBlackboard> _group;
        private readonly TParent _parent;

        public BehaviourTreeGroupBuilder(TParent parent, CompositeNode<TBlackboard> group)
        {
            _parent = parent;
            _group = group;
        }

        protected override BehaviourTreeGroupBuilder<TBlackboard, TParent> HandleNode(
            BehaviourTreeNode<TBlackboard> node)
        {
            _group.AddChild(node);
            return this;
        }

        protected override BehaviourTreeGroupBuilder<TBlackboard, TParent> GetThisAsParentFor(
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