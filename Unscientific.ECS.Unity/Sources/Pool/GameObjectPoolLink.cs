using UnityEngine;

namespace Unscientific.ECS.Unity
{
    public class GameObjectPoolLink: MonoBehaviour
    {
        public GameObjectPool Pool;
    }

    public static class GameObjectPoolExtensions {
        public static GameObjectPool GetPool(this GameObject gameObject)
        {
            return gameObject.GetComponent<GameObjectPoolLink>().Pool;
        }
        
        public static void LinkToPool(this GameObject gameObject, GameObjectPool pool)
        {
            gameObject.AddComponent<GameObjectPoolLink>().Pool = pool;
        }

        public static void ReturnToPool(this GameObject gameObject)
        {
            gameObject.GetPool().Return(gameObject);
        }
    }
}