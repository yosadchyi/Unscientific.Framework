namespace Unscientific.ECS
{
    public class ContextReachedMaximalCapacity<TScope> : System.Exception
    {
        public ContextReachedMaximalCapacity(int maxCapacity) :
            base(string.Format("Context {0} reached maximal capacity {1}!", typeof(TScope).Name, maxCapacity))
        {
        }
    }
}