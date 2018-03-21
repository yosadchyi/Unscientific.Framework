using System.Collections.Generic;
using UnityEngine;

namespace Unscientific.ECS.Unity
{
    public class AssetFactory
    {
        private readonly Transform _parentTransform;
        private readonly Dictionary<string, GameObjectPool> _assetNameToPool = new Dictionary<string, GameObjectPool>();

        public AssetFactory(Transform parentTransform)
        {
            _parentTransform = parentTransform;
        }

        public GameObject CreateAsset(string name)
        {
            GameObjectPool gameObjectPool;

            if (!_assetNameToPool.TryGetValue(name, out gameObjectPool))
            {
                gameObjectPool = new GameObjectPool(name, _parentTransform);
                _assetNameToPool[name] = gameObjectPool;
            }

            return gameObjectPool.Get();
        }
    }
}