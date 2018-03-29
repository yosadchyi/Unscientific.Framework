using UnityEngine;

namespace Unscientific.ECS.Unity
{
    public class EntityLink<TScope>: MonoBehaviour where TScope : IScope
    {
        public Entity<TScope> Entity;
        public int EntityId;
    }
}