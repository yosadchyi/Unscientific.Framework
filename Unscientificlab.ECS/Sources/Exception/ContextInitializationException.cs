namespace Unscientificlab.ECS.Exception
{
    public class ContextInitializationException<TScope> : global::System.Exception
    {
        public ContextInitializationException(): base(string.Format("Context {0} is already initialized!", typeof(TScope).Name))
        {
        }
    }
}