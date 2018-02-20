using System;
using Unscientificlab.ECS.Exception;

namespace Unscientificlab.ECS.ReferenceTracking
{
    public class CountingReferenceTracker : IReferenceTracker
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
            return _references[id];
        }

        public void Release(object owner, int id)
        {
            if (_references[id] == 0)
            {
                // ReSharper disable once HeapView.ObjectAllocation.Evident
                throw new ReleasingNonRetainedEntityException(id);
            }
            _references[id]--;
        }

        public void Retain(object owner, int id)
        {
            _references[id]++;
        }
    }
}