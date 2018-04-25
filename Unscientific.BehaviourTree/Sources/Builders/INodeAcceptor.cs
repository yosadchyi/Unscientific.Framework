namespace Unscientific.BehaviourTree
{
    public interface INodeAcceptor<TBlackboard>
    {
        BehaviourTreeNode<TBlackboard> AcceptNode(BehaviourTreeNode<TBlackboard> node);
    }
}