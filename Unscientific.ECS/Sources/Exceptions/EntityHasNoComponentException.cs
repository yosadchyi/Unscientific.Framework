namespace Unscientific.ECS
{
    public class EntityHasNoComponentException<TScope, TComponent> : System.Exception
    {
        public EntityHasNoComponentException(int id) :
            base($"Entity {typeof(TScope).Name}#{id} has no component {typeof(TComponent).Name}!")
        {
        }
    }
}