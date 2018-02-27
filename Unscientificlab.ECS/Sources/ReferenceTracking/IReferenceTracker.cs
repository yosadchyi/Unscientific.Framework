namespace Unscientificlab.ECS.ReferenceTracking
{
    public interface IReferenceTracker
    {
        void Grow(int newCapacity);
        int RetainCount(int id);
        void Release(int id, object owner);
        void Retain(int id, object owner);
    }
}