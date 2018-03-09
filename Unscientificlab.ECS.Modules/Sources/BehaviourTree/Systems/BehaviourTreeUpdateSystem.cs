﻿using Unscientificlab.ECS.Modules.Core;

namespace Unscientificlab.ECS.Modules.BehaviourTree
{
    public class BehaviourTreeUpdateSystem: IUpdateSystem
    {
        private readonly Context<Simulation> _simulation;

        public BehaviourTreeUpdateSystem(Contexts contexts)
        {
            _simulation = contexts.Get<Simulation>();
        }

        public void Update()
        {
            foreach (var entity in _simulation.AllWith<BehaviourTreeData>())
            {
                if (entity.Is<Destroyed>())
                    continue;

                entity.Get<BehaviourTreeData>().BehaviourTree.Execute(entity);
            }
        }
    }
}