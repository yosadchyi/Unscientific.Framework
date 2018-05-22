using System;

namespace Unscientific.ECS.DSL
{
    public class ContextListBuilder: NestedBuilder<FeatureBuilder>
    {
        public class Configurer
        {
            internal int InitialCapacity = 128;
            internal int MaxCapacity = int.MaxValue;

            public Configurer SetInitialCapacity(int initialCapacity)
            {
                InitialCapacity = initialCapacity;
                return this;
            }
        
            public Configurer SetMaxCapacity(int maxCapacity)
            {
                MaxCapacity = maxCapacity;
                return this;
            }

            public Configurer AllowOnlyOneEntity()
            {
                InitialCapacity = 1;
                MaxCapacity = 1;
                return this;
            }
        }
        
        private readonly Action<ContextElement> _consume;

        internal ContextListBuilder(FeatureBuilder parent, Action<ContextElement> consume) : base(parent)
        {
            _consume = consume;
        }

        public ContextListBuilder Add<TScope>()
        {
            return Add<TScope>(c => c);
        }

        public ContextListBuilder Add<TScope>(Func<Configurer, Configurer> configure)
        {
            var configurer = configure(new Configurer());
            // IL2CPP hack to have appropriate type instance
            Context<TScope>.InstantiateType();
            _consume(new ContextElement(typeof(TScope), configurer.InitialCapacity, configurer.MaxCapacity));
            return this;
        } 
    }
}