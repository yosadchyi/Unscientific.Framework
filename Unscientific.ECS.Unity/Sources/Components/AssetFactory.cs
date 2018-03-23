using System.Collections.Generic;
using UnityEngine;

namespace Unscientific.ECS.Unity
{
    public class AssetFactory: MonoBehaviour, IHandler
    {
        public Transform ParentTransform;
        public bool ClearPoolsOnDestroy;

        private readonly Dictionary<string, GameObjectPool> _assetNameToPool = new Dictionary<string, GameObjectPool>();

        public void Initialize(Contexts contexts, MessageBus messageBus)
        {
            // default to root game object transform
            if (ParentTransform == null)
                ParentTransform = transform;
        }

        public GameObject CreateAsset(string name)
        {
            GameObjectPool gameObjectPool;

            if (!_assetNameToPool.TryGetValue(name, out gameObjectPool))
            {
                gameObjectPool = new GameObjectPool(name, ParentTransform);
                _assetNameToPool[name] = gameObjectPool;
            }

            return gameObjectPool.Get();
        }

        public void Destroy()
        {
            if (ClearPoolsOnDestroy)
            {
                foreach (var pool in _assetNameToPool.Values)
                    pool.Clear();
            }
        }
    }
}