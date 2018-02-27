namespace Unscientificlab.BehaviourTree
{
    public abstract class AbstractCondition<TBlackboard> : IAction<TBlackboard>
    {
        public BehaviourTreeStatus Execute(TBlackboard blackboard)
        {
            return Condition(blackboard) ? BehaviourTreeStatus.Success : BehaviourTreeStatus.Failure;
        }

        public abstract bool Condition(TBlackboard blackboard);
    }
}