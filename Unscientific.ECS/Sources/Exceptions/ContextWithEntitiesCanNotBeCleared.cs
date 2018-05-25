namespace Unscientific.ECS
{
    public class ContextWithEntitiesCanNotBeCleared<TScope> : System.Exception
    {
        public ContextWithEntitiesCanNotBeCleared() :
            base($"Context {typeof(TScope).Name} has entities and can not be cleared!")
        {
        }
    }
}