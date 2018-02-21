namespace Unscientificlab.ECS
{
    public class Contexts
    {
        public static Context<TScope> Get<TScope>() where TScope: IScope
        {
            return Context<TScope>.Instance;
        }
    }
}