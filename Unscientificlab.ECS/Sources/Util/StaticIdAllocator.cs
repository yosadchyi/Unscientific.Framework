namespace Unscientificlab.ECS
{
    // ReSharper disable once UnusedTypeParameter
    public static class StaticIdAllocator<TIdHolder>
    {
        // ReSharper disable once StaticMemberInGenericType
        private static int _lastId;

        public static int AllocateId()
        {
            return _lastId++;
        }
    }
}