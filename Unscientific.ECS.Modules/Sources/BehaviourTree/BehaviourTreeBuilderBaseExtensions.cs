using Unscientific.BehaviourTree;

namespace Unscientific.ECS.Modules.BehaviourTree
{
    public static class BehaviourTreeBuilderBaseExtensions
    {
        public static TResult Wait<TBlackboard, TResult, TParent>(this BehaviourTreeBuilderBase<TBlackboard, TResult, TParent> self, string name, int framesToWait) where TParent : INodeAcceptor<TBlackboard>
        {
            return self.Wait(name, framesToWait, new TickSupplier(World.Instance.Contexts));
        }

        public static TResult Wait<TBlackboard, TResult, TParent>(this BehaviourTreeBuilderBase<TBlackboard, TResult, TParent> self, string name, IValueSupplier<TBlackboard, int> framesToWaitSupplier) where TParent : INodeAcceptor<TBlackboard>
        {
            return self.Wait(name, framesToWaitSupplier, new TickSupplier(World.Instance.Contexts));
        }
    }
}