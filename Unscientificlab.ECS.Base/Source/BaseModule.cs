namespace Unscientificlab.ECS.Base
{
    public class BaseModule : IModule
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
                .For<Simulation>()
                    .Add<Destroyed>()
                .End();
        }

        public Systems Systems(Contexts contexts, MessageBus bus)
        {
            return new Systems.Builder()
                .Add(new SetupSystem(contexts))
                .Add(new DestroySystem<Simulation>(contexts, bus))
                .Build();
        }
    }
}