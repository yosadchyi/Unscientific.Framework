namespace Unscientific.ECS.Features.Core
{
    public static class DestroyEntityExtensions
    {
        public static void Destroy<TScope>(this Entity<TScope> self)
        {
            if (!self.Has<Destroyed>())
            {
                self.Add(new Destroyed());
            }
        }
    }
}