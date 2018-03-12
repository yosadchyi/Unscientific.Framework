using System;

namespace Unscientific.BehaviourTree
{
    public class ActionNode<TBlackboard> : BehaviourTreeNode<TBlackboard>
    {
        private readonly Func<TBlackboard, BehaviourTreeStatus> _updateFn;

        public ActionNode(string name, Func<TBlackboard, BehaviourTreeStatus> updateFn) : base(name)
        {
            _updateFn = updateFn;

            if (_updateFn == null)
                throw new ArgumentNullException(string.Format("[{0}] Tick delegate is null", name));
        }

        protected override BehaviourTreeStatus Update(BehaviourTreeExecutor<TBlackboard> executor, BehaviourTreeExecutionData<TBlackboard> data, TBlackboard blackboard)
        {
            return _updateFn(blackboard);
        }
    }
}