using Unscientificlab.ECS;
using Unscientificlab.ECS.Base;

namespace Unscientificlab.Core.Time
{
    public class IncrementTickSystem: IUpdateSystem
    {
        private Entity<Singletons> _singleton;

        public IncrementTickSystem(Contexts contexts)
        {
            _singleton = contexts.Get<Singletons>().First();
        }

        public void Update()
        {
            var value = _singleton.Get<Tick>().Value;

            _singleton.Replace(new Tick(value + 1));
        }
    }
}