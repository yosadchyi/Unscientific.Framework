﻿using Unscientific.ECS.Modules.Core;

namespace Unscientific.ECS.Modules.BehaviourTree
{
    public class BehaviourTreeSetupSystem: ISetupSystem
    {
        private readonly Context<Configuration> _configuration;
        private readonly int _updatePeriodInTicks;

        public BehaviourTreeSetupSystem(Contexts contexts, int updatePeriodInTicks)
        {
            _configuration = contexts.Get<Configuration>();
            _updatePeriodInTicks = updatePeriodInTicks;
        }

        public void Setup()
        {
            _configuration.Singleton().Add(new BehaviourTreeUpdatePeriod(_updatePeriodInTicks));
        }
    }
}