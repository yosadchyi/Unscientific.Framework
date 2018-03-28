namespace Unscientific.BehaviourTree
{
    public class ConstantValueSupplier<TBlackboard, T>: IValueSupplier<TBlackboard, T>
    {
        private readonly T _value;

        public ConstantValueSupplier(T value)
        {
            _value = value;
        }

        public T Supply(TBlackboard blackboard)
        {
            return _value;
        }
    }
}