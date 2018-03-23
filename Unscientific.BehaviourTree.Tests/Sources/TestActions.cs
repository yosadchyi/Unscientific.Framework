namespace Unscientific.BehaviourTree.Tests
{
    public static class TestActions
    {
        public static BehaviourTreeStatus IncrementCounter1(TestBlackboard blackboard)
        {
            blackboard.Counter1++;
            return BehaviourTreeStatus.Success;
        }

        public static BehaviourTreeStatus IncrementCounter2(TestBlackboard blackboard)
        {
            blackboard.Counter2++;
            return BehaviourTreeStatus.Success;
        }

        public static bool False(TestBlackboard blackboard)
        {
            return false;
        }

        public static bool True(TestBlackboard blackboard)
        {
            return true;
        }

        public static BehaviourTreeStatus ReachLimitAction(TestBlackboard blackboard)
        {
            IncrementCounter1(blackboard);
            return blackboard.Counter1 == 3 ? BehaviourTreeStatus.Success : BehaviourTreeStatus.Failure;
        }
    }
}