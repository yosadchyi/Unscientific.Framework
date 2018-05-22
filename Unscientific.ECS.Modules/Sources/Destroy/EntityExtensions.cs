﻿namespace Unscientific.ECS.Features.Destroy
{
    public static class EntityExtensions
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