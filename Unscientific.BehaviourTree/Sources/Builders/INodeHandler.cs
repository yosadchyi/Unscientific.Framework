namespace Unscientific.BehaviourTree
{
    public interface INodeHandler<TBlackboard>
    {
        void DoHandleNode(BehaviourTreeNode<TBlackboard> node);
    }
}