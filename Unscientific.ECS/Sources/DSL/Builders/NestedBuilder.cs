namespace Unscientific.ECS.DSL
{
    public class NestedBuilder<TParent>
    {
        public readonly TParent Parent;

        protected NestedBuilder(TParent parent)
        {
            Parent = parent;
        }

        public virtual TParent End()
        {
            return Parent;
        }
    }
}