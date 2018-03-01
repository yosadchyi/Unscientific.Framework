namespace Unscientificlab.ECS
{
    public interface IModule
    {
        ContextRegistrations Contexts();
        MessageRegistrations Messages();
        ComponentRegistrations Components();
        Systems Systems(Contexts contexts, MessageBus bus);
    }
}