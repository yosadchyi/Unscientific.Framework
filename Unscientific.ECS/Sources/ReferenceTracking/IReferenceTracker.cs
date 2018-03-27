namespace Unscientific.ECS
{
    public interface IReferenceTracker
    {
        void Grow(int newCapacity);
        int RetainCount(int id);
        void Release(int id);
        void Retain(int id);
        void Clear();
    }
}