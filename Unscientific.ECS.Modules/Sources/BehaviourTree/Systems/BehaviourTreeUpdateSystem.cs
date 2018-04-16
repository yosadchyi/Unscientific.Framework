using Unscientific.ECS.Modules.Core;

namespace Unscientific.ECS.Modules.BehaviourTree
{
    public class BehaviourTreeUpdateSystem<TScope>: IUpdateSystem where TScope : IScope
    {
        private readonly Context<Configuration> _configuration;
        private readonly Context<Singletons> _singletons;
        private readonly Context<TScope> _context;

        public BehaviourTreeUpdateSystem(Contexts contexts)
        {
            _configuration = contexts.Get<Configuration>();
            _singletons = contexts.Get<Singletons>();
            _context = contexts.Get<TScope>();
        }

        public void Update()
        {
            var updatePeriod = _configuration.Singleton().Get<BehaviourTreeUpdatePeriod<TScope>>().PeriodInTicks;
            var tick = _singletons.Singleton().Get<Tick>().Value;

            if (tick % updatePeriod != 0) return;

            foreach (var entity in _context.AllWith<BehaviourTreeData<TScope>>())
            {
                if (!entity.Is<Destroyed>()) entity.Get<BehaviourTreeData<TScope>>().BehaviourTree.Execute(entity);
            }
        }
    }
}