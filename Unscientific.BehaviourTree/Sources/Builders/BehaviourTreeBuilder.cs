namespace Unscientific.BehaviourTree
{
    public class BehaviourTreeBuilder<TBlackboard> :
        BehaviourTreeBuilderBase<TBlackboard, BehaviourTreeBuilderLeaf<TBlackboard>,
            BehaviourTreeBuilderLeaf<TBlackboard>>
    {
        public override BehaviourTreeNode<TBlackboard> AcceptNode(BehaviourTreeNode<TBlackboard> node)
        {
            return node;
        }

        protected override BehaviourTreeBuilderLeaf<TBlackboard> ConvertNodeToResult(BehaviourTreeNode<TBlackboard> node)
        {
            return new BehaviourTreeBuilderLeaf<TBlackboard>(node);
        }

        protected override BehaviourTreeBuilderLeaf<TBlackboard> GetThisForNode(BehaviourTreeNode<TBlackboard> node)
        {
            return new BehaviourTreeBuilderLeaf<TBlackboard>(node);
        }
    }
}