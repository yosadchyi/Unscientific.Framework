namespace Unscientificlab.ECS
{
    public class EntityHasNoComponentException<TScope, TComponent> : System.Exception
    {
        public EntityHasNoComponentException(int id) :
            base(string.Format("Entity {0}#{1} has no component {2}!", typeof(TScope).Name, id, typeof(TComponent).Name))
        {
        }
    }
}