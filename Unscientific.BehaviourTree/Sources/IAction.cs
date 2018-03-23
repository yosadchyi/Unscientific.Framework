namespace Unscientific.BehaviourTree
{
    public interface IAction<in TBlackboard>
    {
        BehaviourTreeStatus Execute(TBlackboard blackboard);
    }
}