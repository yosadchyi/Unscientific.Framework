namespace Unscientific.BehaviourTree
{
    public class BehaviourTreeBuilder<TBlackboard> :
        BehaviourTreeBuilderBase<TBlackboard, BehaviourTreeBuilderFinalizer<TBlackboard>>
    {
        private BehaviourTreeNode<TBlackboard> _node;

        protected override void AcceptNode(BehaviourTreeNode<TBlackboard> node)
        {
            _node = node;
        }

        protected override BehaviourTreeBuilderFinalizer<TBlackboard> GetBuilderMethodResult()
        {
            return new BehaviourTreeBuilderFinalizer<TBlackboard>(_node);
        }
    }
}