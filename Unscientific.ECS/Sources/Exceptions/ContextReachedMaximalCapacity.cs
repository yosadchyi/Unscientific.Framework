namespace Unscientific.ECS
{
    public class ContextReachedMaximalCapacity<TScope> : System.Exception
    {
        public ContextReachedMaximalCapacity(int maxCapacity) :
            base($"Context {typeof(TScope).Name} reached maximal capacity {maxCapacity}!")
        {
        }
    }
}