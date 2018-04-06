using Unscientific.ECS.Modules.Core;

namespace Unscientific.ECS.Modules.BehaviourTree
{
    public class BehaviourTreeUpdateSystem: IUpdateSystem
    {
        private readonly Context<Configuration> _configuration;
        private readonly Context<Singletons> _singletons;
        private readonly Context<Game> _game;

        public BehaviourTreeUpdateSystem(Contexts contexts)
        {
            _configuration = contexts.Get<Configuration>();
            _singletons = contexts.Get<Singletons>();
            _game = contexts.Get<Game>();
        }

        public void Update()
        {
            var updatePeriod = _configuration.Singleton().Get<BehaviourTreeUpdatePeriod>().PeriodInTicks;
            var tick = _singletons.Singleton().Get<Tick>().Value;

            if (tick % updatePeriod != 0) return;
            
            foreach (var entity in _game.AllWith<BehaviourTreeData>())
            {
                if (!entity.Is<Destroyed>())
                    entity.Get<BehaviourTreeData>().BehaviourTree.Execute(entity);
            }
        }
    }
}