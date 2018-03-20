using System.Collections.Generic;
using UnityEngine;
using Unscientific.ECS.Modules.Core;
using Unscientific.ECS.Modules.Physics;
using Unscientific.ECS.Modules.View;
using Transform = UnityEngine.Transform;

namespace Unscientific.ECS.Unity
{
    public class ViewHandler<TScope>: 
        IComponentListener<TScope, View>,
        IComponentListener<TScope, Position>,
        IComponentListener<TScope, Orientation>
        where TScope : IScope
    {
        private readonly Contexts _contexts;
        private readonly Transform _parentTransform;
        private readonly Dictionary<string, GameObjectPool> _assetToPool = new Dictionary<string, GameObjectPool>();
        private readonly Dictionary<int, GameObject> _entityToView = new Dictionary<int, GameObject>();

        public ViewHandler(Contexts contexts, Transform parentTransform)
        {
            _contexts = contexts;
            _parentTransform = parentTransform;
            _contexts.Singleton()
                .AddComponentListener<TScope, View>(this)
                .AddComponentListener<TScope, Position>(this)
                .AddComponentListener<TScope, Orientation>(this);
        }

        public void Destroy()
        {
            _contexts.Singleton()
                .RemoveComponentListener<TScope, View>(this)
                .RemoveComponentListener<TScope, Position>(this)
                .RemoveComponentListener<TScope, Orientation>(this);
        }

        public GameObject GetEntityView(Entity<TScope> entity)
        {
            return _entityToView[entity.Id];
        }

        #region View Listener

        public void OnComponentAdded(Entity<TScope> entity, View view)
        {
            _entityToView[entity.Id] = GetView(view.Name);
            if (entity.Has<Position>()) MoveView(entity, entity.Get<Position>());
            if (entity.Has<Orientation>()) RotateView(entity, entity.Get<Orientation>());
        }

        public void OnComponentRemoved(Entity<TScope> entity, View component)
        {
            var gameObject = _entityToView[entity.Id];
            _entityToView.Remove(entity.Id);
            gameObject.ReturnToPool();
        }

        public void OnComponentReplaced(Entity<TScope> entity, View oldComponent, View newComponent)
        {
            OnComponentRemoved(entity, oldComponent);
            OnComponentAdded(entity, newComponent);
        }

        #endregion

        #region Position Listener

        public void OnComponentAdded(Entity<TScope> entity, Position position)
        {
            if (entity.Has<View>()) MoveView(entity, position);
        }

        public void OnComponentRemoved(Entity<TScope> entity, Position component)
        {
        }

        public void OnComponentReplaced(Entity<TScope> entity, Position oldPosition, Position newPosition)
        {
            if (entity.Has<View>()) MoveView(entity, newPosition);
        }

        #endregion

        #region Orientation Listener

        public void OnComponentAdded(Entity<TScope> entity, Orientation orientation)
        {
            if (entity.Has<View>()) RotateView(entity, orientation);
        }

        public void OnComponentRemoved(Entity<TScope> entity, Orientation orientation)
        {
        }

        public void OnComponentReplaced(Entity<TScope> entity, Orientation oldOrientation, Orientation newOrientation)
        {
            if (entity.Has<View>()) RotateView(entity, newOrientation);
        }

        #endregion

        private GameObject GetView(string name)
        {
            GameObjectPool gameObjectPool;

            if (!_assetToPool.TryGetValue(name, out gameObjectPool))
            {
                gameObjectPool = new GameObjectPool(name, _parentTransform);
                _assetToPool[name] = gameObjectPool;
            }

            return gameObjectPool.Get();
        }
        
        private void MoveView(Entity<TScope> entity, Position position)
        {
            GameObject view;

            if (_entityToView.TryGetValue(entity.Id, out view))
                view.transform.localPosition = position.Value.ToVector3();
        }

        private void RotateView(Entity<TScope> entity, Orientation orientation)
        {
            GameObject view;

            if (_entityToView.TryGetValue(entity.Id, out view))
                view.transform.eulerAngles = new Vector3(0, -orientation.Value.AsFloat * Mathf.Rad2Deg, 0);
        }
    }
}