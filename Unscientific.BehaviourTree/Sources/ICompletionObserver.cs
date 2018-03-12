namespace Unscientific.BehaviourTree
{
    public interface ICompletionObserver<TBlackboard>
    {
        void OnComplete(BehaviourTreeExecutor<TBlackboard> executor,
            BehaviourTreeExecutionData<TBlackboard> execution,
            BehaviourTreeStatus status);
    }
}