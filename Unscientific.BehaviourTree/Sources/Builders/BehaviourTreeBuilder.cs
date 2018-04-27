namespace Unscientific.BehaviourTree
{
    public class BehaviourTreeBuilder<TBlackboard> :
        BehaviourTreeBuilderBase<TBlackboard, BehaviourTreeBuilderFinalizer<TBlackboard>,
            BehaviourTreeBuilderFinalizer<TBlackboard>>
    {
        public override BehaviourTreeNode<TBlackboard> AcceptNode(BehaviourTreeNode<TBlackboard> node)
        {
            return node;
        }

        protected override BehaviourTreeBuilderFinalizer<TBlackboard> ConvertNodeToResult(BehaviourTreeNode<TBlackboard> node)
        {
            return new BehaviourTreeBuilderFinalizer<TBlackboard>(node);
        }

        protected override BehaviourTreeBuilderFinalizer<TBlackboard> GetThisForNode(BehaviourTreeNode<TBlackboard> node)
        {
            return new BehaviourTreeBuilderFinalizer<TBlackboard>(node);
        }
    }
}