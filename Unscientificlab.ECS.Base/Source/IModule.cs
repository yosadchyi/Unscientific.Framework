namespace Unscientificlab.ECS.Base
{
    public interface IModule
    {
        ContextRegistrations Contexts();
        MessageRegistrations Messages();
        ComponentRegistrations Components();
        ISystem[] Systems(Contexts contexts, MessageBus bus);
    }
}