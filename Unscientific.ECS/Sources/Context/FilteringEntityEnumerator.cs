namespace Unscientific.ECS
{
    public struct FilteringEntityEnumerator<TScope, TComponent1, TComponent2, TComponent3>
    {
        private readonly int _count;
        private int _current;

        public FilteringEntityEnumerator(int count)
        {
            _count = count;
            _current = -1;
        }

        public void Dispose()
        {
        }

        public bool MoveNext()
        {
            while (++_current < _count)
            {
                if (Current.Has<TComponent2>() && Current.Has<TComponent3>())
                    return true;
            }

            return false;
        }

        public void Reset()
        {
            _current = -1;
        }

        public Entity<TScope> Current
        {
            get
            {
                var id = Context<TScope>.ComponentData<TComponent1>.ComponentIndexToEntityId[_current];

                return new Entity<TScope>(id);
            }
        }
    }

    public struct FilteringEntityEnumerator<TScope, TComponent1, TComponent2>
    {
        private readonly int _count;
        private int _current;

        public FilteringEntityEnumerator(int count)
        {
            _count = count;
            _current = -1;
        }

        public void Dispose()
        {
        }

        public bool MoveNext()
        {
            while (++_current < _count)
            {
                if (Current.Has<TComponent2>())
                    return true;
            }

            return false;
        }

        public void Reset()
        {
            _current = -1;
        }

        public Entity<TScope> Current
        {
            get
            {
                var id = Context<TScope>.ComponentData<TComponent1>.ComponentIndexToEntityId[_current];

                return new Entity<TScope>(id);
            }
        }
    }

    public struct FilteringEntityEnumerator<TScope, TComponent>
    {
        private readonly int _count;
        private int _current;

        public FilteringEntityEnumerator(int count)
        {
            _count = count;
            _current = -1;
        }

        public void Dispose()
        {
        }

        public bool MoveNext()
        {
            return ++_current < _count;
        }

        public void Reset()
        {
            _current = -1;
        }

        public Entity<TScope> Current
        {
            get
            {
                var id = Context<TScope>.ComponentData<TComponent>.ComponentIndexToEntityId[_current];

                return new Entity<TScope>(id);
            }
        }
    }
}