namespace Unscientific.ECS
{
    public class EntityDoesNotExistsException<TScope> : System.Exception
    {
        public EntityDoesNotExistsException(int id) : base ($"Entity {typeof(TScope).Name}#{id} does not exists!")
        {
        }
    }
}