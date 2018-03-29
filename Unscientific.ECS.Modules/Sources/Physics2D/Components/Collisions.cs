using System.Collections.Generic;

namespace Unscientific.ECS.Modules.Physics2D
{
    public struct Collisions
    {
        public readonly List<Collision> List;

        public Collisions(List<Collision> list)
        {
            List = list;
        }
    }
}