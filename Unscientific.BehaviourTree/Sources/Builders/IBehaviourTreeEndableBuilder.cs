namespace Unscientific.BehaviourTree
{
    public interface IBehaviourTreeEndableBuilder<TBlackboard, out TParent> : INodeAcceptor<TBlackboard>
        where TParent : INodeAcceptor<TBlackboard>
    {
        TParent End();
    }
}