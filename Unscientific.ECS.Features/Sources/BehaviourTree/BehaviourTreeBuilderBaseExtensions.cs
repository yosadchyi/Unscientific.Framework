using Unscientific.BehaviourTree;

namespace Unscientific.ECS.Features.BehaviourTree
{
    public static class BehaviourTreeBuilderBaseExtensions
    {
        public static TResult Wait<TBlackboard, TResult>(this BehaviourTreeBuilderBase<TBlackboard, TResult> self, string name, int framesToWait)
        {
            return self.Wait(name, framesToWait, new TickSupplier(World.Instance.Contexts));
        }

        public static TResult Wait<TBlackboard, TResult>(this BehaviourTreeBuilderBase<TBlackboard, TResult> self, string name, IValueSupplier<TBlackboard, int> framesToWaitSupplier)
        {
            return self.Wait(name, framesToWaitSupplier, new TickSupplier(World.Instance.Contexts));
        }
        
        public static SimpleBehaviourTreeBuilder<TBlackboard> Wait<TBlackboard>(this SimpleBehaviourTreeBuilder<TBlackboard> self, string name, int framesToWait)
        {
            return self.Wait(name, framesToWait, new TickSupplier(World.Instance.Contexts));
        }

        public static SimpleBehaviourTreeBuilder<TBlackboard> Wait<TBlackboard>(this SimpleBehaviourTreeBuilder<TBlackboard> self, string name, IValueSupplier<TBlackboard, int> framesToWaitSupplier)
        {
            return self.Wait(name, framesToWaitSupplier, new TickSupplier(World.Instance.Contexts));
        }
    }
}