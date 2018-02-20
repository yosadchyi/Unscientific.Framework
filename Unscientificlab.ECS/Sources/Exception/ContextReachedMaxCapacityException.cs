namespace Unscientificlab.ECS.Exception
{
    public class ContextReachedMaxCapacityException<TScope> : global::System.Exception
    {
        public ContextReachedMaxCapacityException() : base(string.Format("Entity pool {0} exhaused!", typeof(TScope).Name))
        {
        }
    }
}