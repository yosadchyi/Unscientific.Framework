namespace Unscientificlab.ECS.ReferenceTracking
{
    public interface IReferenceTracker
    {
        void Grow(int newCapacity);
        int RetainCount(int id);
        void Release(object owner, int id);
        void Retain(object owner, int id);
    }
}