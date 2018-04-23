namespace Unscientific.BehaviourTree
{
    public interface IBehaviourTreeEndableBuilder<TBlackboard, out TParent> : INodeHandler<TBlackboard>
        where TParent : INodeHandler<TBlackboard>
    {
        TParent End();
    }
}