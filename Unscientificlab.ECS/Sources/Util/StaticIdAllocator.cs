namespace Unscientificlab.ECS.Util
{
    // ReSharper disable once UnusedTypeParameter
    public class StaticIdAllocator<TIdHolder>
    {
        // ReSharper disable once StaticMemberInGenericType
        private static int _lastId;

        public static int AllocateId()
        {
            return _lastId++;
        }
    }
}