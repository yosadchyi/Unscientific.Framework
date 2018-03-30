namespace Unscientific.ECS
{
    public class EntityAlreadyHasComponentException<TScope, TComponent> : System.Exception
    {
        public EntityAlreadyHasComponentException(int id) :
            base($"Entity {typeof(TScope).Name}#{id} already has component {typeof(TComponent).Name}!")
        {
        }
    }
}