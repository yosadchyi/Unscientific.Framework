namespace Unscientific.ECS
{
    public struct FilteringEntityEnumerable<TScope, TComponent1, TComponent2, TComponent3>
    {
    
        private readonly int _count;

        public FilteringEntityEnumerable(int count)
        {
            _count = count;
        }

        public FilteringEntityEnumerator<TScope, TComponent1, TComponent2, TComponent3> GetEnumerator()
        {
            return new FilteringEntityEnumerator<TScope, TComponent1, TComponent2, TComponent3>(_count);
        }
        
        public Entity<TScope> First()
        {
            var enumerator = GetEnumerator();

            if (enumerator.MoveNext())
                return enumerator.Current;

            throw new NoEntitiesException();
        }
    }

    public struct FilteringEntityEnumerable<TScope, TComponent1, TComponent2>
    {
    
        private readonly int _count;

        public FilteringEntityEnumerable(int count)
        {
            _count = count;
        }

        public FilteringEntityEnumerator<TScope, TComponent1, TComponent2> GetEnumerator()
        {
            return new FilteringEntityEnumerator<TScope, TComponent1, TComponent2>(_count);
        }
        
        public Entity<TScope> First()
        {
            var enumerator = GetEnumerator();

            if (enumerator.MoveNext())
                return enumerator.Current;

            throw new NoEntitiesException();
        }
    }

    public struct FilteringEntityEnumerable<TScope, TComponent>
    {
    
        private readonly int _count;

        public FilteringEntityEnumerable(int count)
        {
            _count = count;
        }

        public FilteringEntityEnumerator<TScope, TComponent> GetEnumerator()
        {
            return new FilteringEntityEnumerator<TScope, TComponent>(_count);
        }
        
        public Entity<TScope> First()
        {
            var enumerator = GetEnumerator();

            if (enumerator.MoveNext())
                return enumerator.Current;

            throw new NoEntitiesException();
        }
    }
}