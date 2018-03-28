namespace Unscientific.ECS.Modules.Core
{
    public class IncrementTickSystem: IUpdateSystem
    {
        private readonly Context<Singletons> _singletons;

        public IncrementTickSystem(Contexts contexts)
        {
            _singletons = contexts.Get<Singletons>();
        }

        public void Update()
        {
            var value = _singletons.Singleton().Get<Tick>().Value;

            _singletons.Singleton().Replace(new Tick(value + 1));
        }
    }
}