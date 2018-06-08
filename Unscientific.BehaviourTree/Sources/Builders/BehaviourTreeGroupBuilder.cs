namespace Unscientific.BehaviourTree
{
    public class BehaviourTreeGroupBuilder<TBlackboard, TFinalizeResult> :
        BehaviourTreeBuilderBase<TBlackboard, BehaviourTreeGroupBuilder<TBlackboard, TFinalizeResult>>
    {
        private readonly CompositeNode<TBlackboard> _group;
        private readonly TFinalizeResult _parent;

        public BehaviourTreeGroupBuilder(TFinalizeResult parent, CompositeNode<TBlackboard> group)
        {
            _parent = parent;
            _group = group;
        }

        protected override void AcceptNode(BehaviourTreeNode<TBlackboard> node)
        {
            _group.AddChild(node);
        }

        protected override BehaviourTreeGroupBuilder<TBlackboard, TFinalizeResult> GetBuilderMethodResult()
        {
            return this;
        }

        public TFinalizeResult End()
        {
            return _parent;
        }
    }
}