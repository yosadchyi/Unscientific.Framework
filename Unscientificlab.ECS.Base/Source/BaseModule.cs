namespace Unscientificlab.ECS.Base
{
    public class BaseModule<Simulation> : IModule where Simulation : IScope
    {
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
                    .Add<EntityDestroyed<Simulation>>()
                .End();
        }

        public ISystem[] Systems(Contexts contexts, MessageBus bus)
        {
            return new ISystem[]
            {
                new DestroySystem<Simulation>(contexts, bus), 
            };
        }
    }
}