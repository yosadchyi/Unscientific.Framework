﻿namespace Unscientificlab.ECS.Modules.Base
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
            var value = _singletons.First().Get<Tick>().Value;

            _singletons.First().Replace(new Tick(value + 1));
        }
    }
}