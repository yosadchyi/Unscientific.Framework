namespace Unscientificlab.ECS
{
    public class EntityIsAlreadyRetainedException<TScope> : System.Exception
    {
        public EntityIsAlreadyRetainedException(object owner, int id) : base(string.Format("Entity {0}#{1} is already owned by {2}!", typeof(TScope).Name, id, owner))
        {
        }
    }
}