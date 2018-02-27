using System.Collections.Generic;
using Unscientificlab.ECS.Exception;

namespace Unscientificlab.ECS.ReferenceTracking
{
    public class SafeReferenceTracker<TScope>: IReferenceTracker
    {
        private readonly Dictionary<int, HashSet<object>> _references;

        public SafeReferenceTracker(int capacity)
        {
            _references = new Dictionary<int, HashSet<object>>(capacity);
        }

        public void Grow(int newCapacity)
        {
            // nothing to do
        }

        public int RetainCount(int id)
        {
            HashSet<object> owners;

            return _references.TryGetValue(id, out owners) ? owners.Count : 0;
        }

        public void Release(int id, object owner)
        {
            HashSet<object> owners;

            if (!_references.TryGetValue(id, out owners) || !owners.Contains(owner))
                throw new ReleasingNonOwnedEntityException<TScope>(owner, id);

            owners.Remove(owner);
        }

        public void Retain(int id, object owner)
        {
            HashSet<object> owners;

            if (!_references.TryGetValue(id, out owners))
            {
                owners = new HashSet<object>();
                _references[id] = owners;
            }
                
            if (owners.Contains(owner))
                throw new EntityIsAlreadyRetainedException<TScope>(owner, id);

            owners.Add(owner);
        }
    }
}