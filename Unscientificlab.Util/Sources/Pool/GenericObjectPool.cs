namespace Unscientificlab.Util.Pool
{
    public class GenericObjectPool<T>: ObjectPool<T> where T: new()
    {
        protected GenericObjectPool()
        {
        }

        public GenericObjectPool(int instances)
        {
            PrepareInstances(instances);
        }

        protected override T CreateInstance()
        {
            // ReSharper disable once HeapView.ObjectAllocation.Possible
            return new T();
        }
    }
}