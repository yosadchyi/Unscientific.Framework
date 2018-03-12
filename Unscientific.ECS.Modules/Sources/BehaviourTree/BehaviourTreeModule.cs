using Unscientific.ECS.Modules.Core;

namespace Unscientific.ECS.Modules.BehaviourTree
{
    public abstract class BehaviourTreeModule: AbstractModule
    {
        public override ModuleImports Imports()
        {
            return base.Imports()
                .Import<CoreModule>();
        }

        public override ComponentRegistrations Components()
        {
            return base.Components()
                .For<Simulation>()
                    .Add<BehaviourTreeData>()
                .End();
        }

        public override Systems Systems(Contexts contexts, MessageBus bus)
        {
            return new Systems.Builder()
                .Add(new BehaviourTreeUpdateSystem(contexts))
                .Add(new BehaviourTreeCleanupSystem(bus))
                .Build();
        }
    }
}