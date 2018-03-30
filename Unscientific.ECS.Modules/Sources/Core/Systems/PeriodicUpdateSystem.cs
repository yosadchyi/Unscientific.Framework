namespace Unscientific.ECS.Modules.Core
{
    public class PeriodicUpdateSystem: IUpdateSystem
    {
        private readonly IUpdateSystem _decoratedSystem;
        private readonly int _period;
        private readonly Context<Singletons> _singletons;

        public PeriodicUpdateSystem(Contexts contexts, IUpdateSystem decoratedSystem, int period)
        {
            _singletons = contexts.Get<Singletons>();
            _decoratedSystem = decoratedSystem;
            _period = period;
        }

        public void Update()
        {
            var tick = _singletons.Singleton().Get<Tick>().Value;

            if (tick % _period == 0) _decoratedSystem.Update();
        }
    }
}