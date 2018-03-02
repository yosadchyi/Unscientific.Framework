namespace Unscientificlab.ECS.Modules.Base
{
    public static class EntityExtensions
    {
        public static void Destroy<TScope>(this Entity<TScope> self) where TScope : IScope
        {
            if (!self.Has<Destroyed>())
            {
                self.Add(new Destroyed());
                MessageBus.Instance.Send(new EntityDestroyed<TScope>(self));
            }
        }
    }
}