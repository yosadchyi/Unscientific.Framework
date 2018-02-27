using Unscientificlab.Logging;

namespace Unscientificlab.BehaviourTree
{
    public abstract class BehaviourTreeNode<TBlackboard>
    {
        public int Id { get; private set; }
        public ICompletionObserver<TBlackboard> Observer { get; set; }
        public string Name { get; private set; }

        public bool IsTerminated(BehaviourTreeExecutionData<TBlackboard> executionData)
        {
            var status = executionData.Statuses[Id];

            return status == BehaviourTreeStatus.Success || status == BehaviourTreeStatus.Failure;
        }

        public bool IsRunning(BehaviourTreeExecutionData<TBlackboard> executionData)
        {
            return executionData.Statuses[Id] == BehaviourTreeStatus.Running;
        }

        protected BehaviourTreeNode(string name)
        {
            Name = name;
        }

        public virtual void DeclareNode(BehaviourTreeMetadata<TBlackboard> metadata)
        {
            Id = metadata.DeclareNode(this);
        }

        public virtual void Initialize(BehaviourTreeExecutor<TBlackboard> executor, BehaviourTreeExecutionData<TBlackboard> data, TBlackboard blackboard)
        {
            // nothing to do
        }

        public virtual void OnTerminate(BehaviourTreeExecutionData<TBlackboard> behaviourTreeExecutor, BehaviourTreeStatus status)
        {
        }

        protected abstract BehaviourTreeStatus Update(BehaviourTreeExecutor<TBlackboard> executor, BehaviourTreeExecutionData<TBlackboard> data, TBlackboard blackboard);

        public BehaviourTreeStatus Tick(
            BehaviourTreeExecutor<TBlackboard> executor,
            BehaviourTreeExecutionData<TBlackboard> data,
            TBlackboard blackboard)
        {
            Logger.Instance.Log(LogLevel.Trace, "[BT] {0} ", Name);

            if (data.Statuses[Id] != BehaviourTreeStatus.Running)
            {
                data.Statuses[Id] = BehaviourTreeStatus.Uninitialized;
                Initialize(executor, data, blackboard);
            }

            var status = Update(executor, data, blackboard);

            data.Statuses[Id] = status;

            if (status != BehaviourTreeStatus.Running)
                OnTerminate(data, status);

            Logger.Instance.Log(LogLevel.Trace, "[BT] {0} {1}", Name, status);

            return status;
        }

        public void Abort(BehaviourTreeExecutionData<TBlackboard> data)
        {
            data.Statuses[Id] = BehaviourTreeStatus.Aborted;
            OnTerminate(data, BehaviourTreeStatus.Aborted);
        }

        public override string ToString()
        {
            return string.Format("Node({0}, #{1})", Name, Id);
        }
    }
}