using UnityEngine;

namespace Unscientific.ECS.Unity
{
    public class EntityLink<TScope>: MonoBehaviour
    {
        public Entity<TScope> Entity;
        public int EntityId;
    }
}