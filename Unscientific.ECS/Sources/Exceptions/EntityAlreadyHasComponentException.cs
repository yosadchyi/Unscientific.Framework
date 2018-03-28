namespace Unscientific.ECS
{
    public class EntityAlreadyHasComponentException<TScope, TComponent> : System.Exception
    {
        public EntityAlreadyHasComponentException(int id) :
            base(string.Format("Entity {0}#{1} already has component {2}!", typeof(TScope).Name, id, typeof(TComponent).Name))
        {
        }
    }
}