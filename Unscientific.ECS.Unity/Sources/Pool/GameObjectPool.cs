using System;
using UnityEngine;
using Unscientific.Util.Pool;
using Object = UnityEngine.Object;
using Logger = Unscientific.Logging.Logger;
using LogLevel = Unscientific.Logging.LogLevel;

namespace Unscientific.ECS.Unity
{
    public class GameObjectPool : ObjectPool<GameObject>
    {
        private readonly string _assetName;
        private readonly Transform _viewContainer;
        private GameObject _cachedAsset;

        public GameObjectPool(string assetName, Transform viewContainer)
        {
            _assetName = assetName;
            _viewContainer = viewContainer;
        }

        protected override GameObject CreateInstance()
        {
            if (_cachedAsset == null)
            {
                _cachedAsset = Resources.Load<GameObject>(_assetName);
            }

            GameObject gameObject = null;

            try
            {
                gameObject = Object.Instantiate(_cachedAsset);
            }
            catch (Exception)
            {
                Logger.Instance.Log(LogLevel.Error, "Cannot instantiate {0}", _assetName);
            }

            gameObject.LinkToPool(this);

            return gameObject;
        }

        protected override void Activate(GameObject instance)
        {
            instance.transform.SetParent(_viewContainer, false);
            instance.SetActive(true);
        }

        protected override void Deactivate(GameObject instance)
        {
            instance.transform.SetParent(null);
            instance.SetActive(false);
        }
    }
}