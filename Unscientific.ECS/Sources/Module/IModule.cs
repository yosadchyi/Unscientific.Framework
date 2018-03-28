namespace Unscientific.ECS
{
    public interface IModule
    {
        ModuleUsages Usages();
        ContextRegistrations Contexts();
        MessageRegistrations Messages();
        ComponentRegistrations Components();
        NotificationRegistrations Notifications();
        Systems Systems(Contexts contexts, MessageBus bus);
    }
}