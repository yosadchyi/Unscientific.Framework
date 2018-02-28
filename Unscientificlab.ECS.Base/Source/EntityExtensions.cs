namespace Unscientificlab.ECS.Base
{
    public static class EntityExtensions
    {
        public static void Destroy<TScope>(this Entity<TScope> self) where TScope : IScope
        {
            if (!self.Is<Destroyed>())
            {
                self.Add(new Destroyed());
                MessageBus.Instance.Send(new EntityDestroyed<TScope>(self));
            }
        }
    }
}