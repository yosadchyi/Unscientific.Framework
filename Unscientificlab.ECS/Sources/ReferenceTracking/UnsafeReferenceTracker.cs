namespace Unscientificlab.ECS
{
    public class UnsafeReferenceTracker: IReferenceTracker
    {
        public void Grow(int newCapacity)
        {
            // do nothing
        }

        public int RetainCount(int id)
        {
            // do nothing
            return 0;
        }

        public void Release(int id, object owner)
        {
            // do nothing
        }

        public void Retain(int id, object owner)
        {
            // do nothing
        }
    }
}