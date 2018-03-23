using System.Collections.Generic;

namespace Unscientific.Util.Pool
{
    public class DictionaryPool<TKey, TValue> : GenericObjectPool<Dictionary<TKey, TValue>>
    {
        // ReSharper disable once HeapView.ObjectAllocation.Evident
        public static readonly DictionaryPool<TKey, TValue> Instance = new DictionaryPool<TKey, TValue>();

        protected override void Deactivate(Dictionary<TKey, TValue> instance)
        {
            instance.Clear();
        }
    }

    public class HashSetPool<TItemType> : GenericObjectPool<HashSet<TItemType>>
    {
        // ReSharper disable once HeapView.ObjectAllocation.Evident
        public static readonly HashSetPool<TItemType> Instance = new HashSetPool<TItemType>();

        protected override void Deactivate(HashSet<TItemType> instance)
        {
            instance.Clear();
        }
    }
    
    public class ListPool<TItemType> : GenericObjectPool<List<TItemType>>
    {
        // ReSharper disable once HeapView.ObjectAllocation.Evident
        public static readonly ListPool<TItemType> Instance = new ListPool<TItemType>();

        protected override void Deactivate(List<TItemType> instance)
        {
            instance.Clear();
        }
    }
}
