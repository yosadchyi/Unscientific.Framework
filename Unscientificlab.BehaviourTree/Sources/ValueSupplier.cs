namespace Unscientificlab.Logging.Config
{
    public delegate T ValueSupplier<in TBlackboard, out T>(TBlackboard entity);

    public class ConstantValueSupplier<TBlackboard, T>
    {
        private readonly T _value;

        public ConstantValueSupplier(T value)
        {
            _value = value;
        }

        public T Get(TBlackboard entity)
        {
            return _value;
        }
    }
}