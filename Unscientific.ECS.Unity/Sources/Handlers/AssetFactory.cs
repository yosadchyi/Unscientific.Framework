﻿using System.Collections.Generic;
using UnityEngine;

namespace Unscientific.ECS.Unity
{
    public class AssetFactory: MonoBehaviour, IHandler
    {
        public Transform ParentTransform;

        private readonly Dictionary<string, GameObjectPool> _assetNameToPool = new Dictionary<string, GameObjectPool>();

        public void Initialize(Contexts contexts, MessageBus messageBus)
        {
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
        }
    }
}