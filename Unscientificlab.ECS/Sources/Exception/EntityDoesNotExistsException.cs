namespace Unscientificlab.ECS
{
    public class EntityDoesNotExistsException<TScope> : global::System.Exception
    {
        public EntityDoesNotExistsException(int id) : base (string.Format("Entity {0}#{1} does not exists!", typeof(TScope).Name, id))
        {
        }
    }
}