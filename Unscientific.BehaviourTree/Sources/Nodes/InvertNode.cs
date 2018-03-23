using System;

namespace Unscientific.BehaviourTree
{
    public class InverterNode<TBlackboard> : BaseDecoratorNode<TBlackboard>, ICompletionObserver<TBlackboard>
    {
        public InverterNode(string name) : base(name)
        {
        }

        public override void Initialize(BehaviourTreeExecutor<TBlackboard> executor, BehaviourTreeExecutionData<TBlackboard> data, TBlackboard blackboard)
        {
            executor.Start(data, Node, this);
        }

        public void OnComplete(BehaviourTreeExecutor<TBlackboard> executor, BehaviourTreeExecutionData<TBlackboard> execution, BehaviourTreeStatus status)
        {
            executor.Stop(execution, this, InvertStatus(status));
        }

        protected override BehaviourTreeStatus Update(BehaviourTreeExecutor<TBlackboard> executor, BehaviourTreeExecutionData<TBlackboard> data, TBlackboard blackboard)
        {
            return BehaviourTreeStatus.Running;
        }

        private static BehaviourTreeStatus InvertStatus(BehaviourTreeStatus status)
        {
            switch (status)
            {
                case BehaviourTreeStatus.Failure:
                    return BehaviourTreeStatus.Success;

                case BehaviourTreeStatus.Success:
                    return BehaviourTreeStatus.Failure;

                case BehaviourTreeStatus.Uninitialized:
                case BehaviourTreeStatus.Running:
                case BehaviourTreeStatus.Aborted:
                    return status;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}