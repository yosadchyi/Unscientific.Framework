namespace Unscientific.BehaviourTree
{
    public class BehaviourTreeBuilder<TBlackboard> :
        BehaviourTreeBuilderBase<TBlackboard, BehaviourTreeBuilderLeaf<TBlackboard>,
            BehaviourTreeBuilderLeaf<TBlackboard>>
    {
        protected override BehaviourTreeBuilderLeaf<TBlackboard> ConvertNodeToResult(BehaviourTreeNode<TBlackboard> node)
        {
            return new BehaviourTreeBuilderLeaf<TBlackboard>(node);
        }

        protected override BehaviourTreeBuilderLeaf<TBlackboard> GetThisAsParentFor(BehaviourTreeNode<TBlackboard> node)
        {
            return new BehaviourTreeBuilderLeaf<TBlackboard>(node);
        }
    }
}