namespace Unscientific.ECS
{
    public class Contexts
    {
        public Context<TScope> Get<TScope>() where TScope: IScope
        {
            return Context<TScope>.Instance;
        }
    }
}