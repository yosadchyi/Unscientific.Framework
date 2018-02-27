using System;
using Unscientificlab.ECS;

namespace Unscientificlab.ECS
{
    public class CountingReferenceTracker<TScope> : IReferenceTracker
    {
        private int[] _references;

        public CountingReferenceTracker(int maxEntities)
        {
            // ReSharper disable once HeapView.ObjectAllocation.Evident
            _references = new int[maxEntities];
        }

        public void Grow(int newCapacity)
        {
            if (_references.Length < newCapacity)
                Array.Resize(ref _references, newCapacity);
        }

        public int RetainCount(int id)
        {
            return _references[id - 1];
        }

        public void Release(int id, object owner)
        {
            if (_references[id - 1] == 0)
            {
                // ReSharper disable once HeapView.ObjectAllocation.Evident
                throw new ReleasingNonRetainedEntityException<TScope>(id);
            }
            _references[id - 1]--;
        }

        public void Retain(int id, object owner)
        {
            _references[id - 1]++;
        }
    }
}