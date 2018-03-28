namespace Unscientific.BehaviourTree
{
    public interface IValueSupplier<in TBlackboard, out T>
    {
        T Supply(TBlackboard blackboard);
    }
}