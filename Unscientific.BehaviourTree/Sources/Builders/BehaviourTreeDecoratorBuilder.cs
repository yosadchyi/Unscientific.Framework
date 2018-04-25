namespace Unscientific.BehaviourTree
{
    public class BehaviourTreeDecoratorBuilder<TBlackboard, TParent> :
        BehaviourTreeBuilderBase<TBlackboard, IBehaviourTreeEndableBuilder<TBlackboard, TParent>,
            IBehaviourTreeEndableBuilder<TBlackboard, TParent>>,
        IBehaviourTreeEndableBuilder<TBlackboard, TParent>
        where TParent : INodeHandler<TBlackboard>
    {
        private readonly BaseDecoratorNode<TBlackboard> _decorator;
        private readonly TParent _parent;

        public BehaviourTreeDecoratorBuilder(TParent parent, BaseDecoratorNode<TBlackboard> group)
        {
            _parent = parent;
            _decorator = group;
        }

        protected override IBehaviourTreeEndableBuilder<TBlackboard, TParent> ConvertNodeToResult(BehaviourTreeNode<TBlackboard> node)
        {
            _decorator.Node = node;
            return this;
        }

        protected override IBehaviourTreeEndableBuilder<TBlackboard, TParent> GetThisAsParentFor(BehaviourTreeNode<TBlackboard> node)
        {
            return this;
        }

        public TParent End()
        {
            _parent.DoHandleNode(_decorator);
            return _parent;
        }
    }
}
