namespace Unscientific.BehaviourTree
{
    public class BehaviourTreeDecoratorBuilder<TBlackboard, TFinalizeResult> :
        BehaviourTreeBuilderBase<TBlackboard, IBehaviourTreeDecoratorBuilderFinalizer<TFinalizeResult>>,
        IBehaviourTreeDecoratorBuilderFinalizer<TFinalizeResult>
    {
        private readonly BaseDecoratorNode<TBlackboard> _decorator;
        private readonly TFinalizeResult _parent;

        public BehaviourTreeDecoratorBuilder(TFinalizeResult parent, BaseDecoratorNode<TBlackboard> group)
        {
            _parent = parent;
            _decorator = group;
        }

        protected override void AcceptNode(BehaviourTreeNode<TBlackboard> node)
        {
            _decorator.Node = node;
        }

        protected override IBehaviourTreeDecoratorBuilderFinalizer<TFinalizeResult> GetBuilderMethodResult()
        {
            return this;
        }

        public TFinalizeResult End()
        {
            return _parent;
        }
    }
}
