namespace Unscientificlab.ECS
{
    public abstract class AbstractModule: IModule
    {
        public virtual ModuleImports Imports()
        {
            return new ModuleImports();
        }

        public virtual ContextRegistrations Contexts()
        {
            return new ContextRegistrations();
        }

        public virtual MessageRegistrations Messages()
        {
            return new MessageRegistrations();
        }

        public virtual ComponentRegistrations Components()
        {
            return new ComponentRegistrations();
        }

        public abstract Systems Systems(Contexts contexts, MessageBus bus);
    }
}