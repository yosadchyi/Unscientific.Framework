using System.Collections.Generic;

namespace Unscientific.Util.Pool
{
    public abstract class ObjectPool<T>
    {
        private readonly Queue<T> _pool;

        protected ObjectPool()
        {
            // ReSharper disable once HeapView.ObjectAllocation.Evident
            _pool = new Queue<T>();
        }

        public void EnsureHaveInstances(int count)
        {
            var diff = count - _pool.Count;

            if (diff > 0)
                PrepareInstances(diff);
        }

        public void PrepareInstances(int count)
        {
            for (var i = 0; i < count; i++)
            {
                _pool.Enqueue(CreateInstance());
            }
        }

        public T Get()
        {
            var instance = DoGet();

            Activate(instance);
            return instance;
        }

        private T DoGet()
        {
            return _pool.Count > 0 ? _pool.Dequeue() : CreateInstance();
        }

        public void Return(T instance)
        {
            Deactivate(instance);
            _pool.Enqueue(instance);
        }

        protected virtual void Deactivate(T instance)
        {
        }

        protected virtual void Activate(T instance)
        {
        }

        protected abstract T CreateInstance();
    }
}