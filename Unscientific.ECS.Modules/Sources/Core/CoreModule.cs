namespace Unscientific.ECS.Modules.Core
{
    public class CoreModule : IModule
    {
        public ModuleImports Imports()
        {
            return new ModuleImports();
        }

        public ContextRegistrations Contexts()
        {
            return new ContextRegistrations()
                .Add<Simulation>()
                .Add<Configuration>()
                .Add<Singletons>();
        }

        public MessageRegistrations Messages()
        {
            return new MessageRegistrations()
                .Add<EntityDestroyed<Simulation>>();
        }

        public ComponentRegistrations Components()
        {
            return new ComponentRegistrations()
                .For<Singletons>()
                    .Add<Tick>()
                .End()
                .For<Simulation>()
                    .Add<Destroyed>()
                .End();
        }

        public ECS.Systems Systems(Contexts contexts, MessageBus bus)
        {
            return new ECS.Systems.Builder()
                .Add(new BaseSetupSystem(contexts))
                .Add(new IncrementTickSystem(contexts))
                .Add(new DestroySystem<Simulation>(contexts, bus))
                .Build();
        }
    }
}