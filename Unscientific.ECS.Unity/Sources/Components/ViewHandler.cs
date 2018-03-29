using UnityEngine;
using Unscientific.ECS.Modules.Core;
using Unscientific.ECS.Modules.View;

namespace Unscientific.ECS.Unity
{
    public class ViewHandler<TScope>: MonoBehaviour, IHandler, IComponentListener<TScope, View> where TScope : IScope
    {
        public ViewPlane ViewPlane = ViewPlane.XZ;

        private Contexts _contexts;
        private AssetFactory _assetFactory;
        private EntityViewDatabase<TScope> _entityViewDatabase;
        private IOrientationHandler<TScope> _orientationHandler;
        private IPositionHandler<TScope> _positionHandler;

        public void Initialize(Contexts contexts, MessageBus messageBus)
        {
            _contexts = contexts;
            _assetFactory = GetComponent<AssetFactory>();
            _entityViewDatabase = GetComponent<EntityViewDatabase<TScope>>();
            _orientationHandler = GetComponent<IOrientationHandler<TScope>>();
            _positionHandler = GetComponent<IPositionHandler<TScope>>();
            _contexts.Singleton().AddComponentListener(this);
        }

        public void OnComponentAdded(Entity<TScope> entity, View view)
        {
            var asset = _assetFactory.CreateAsset(view.Name);
            _entityViewDatabase.AddView(entity, asset);
            UpdatePositionAndOrientation(entity);
            asset.GetComponent<InterpolatedTransform>()?.OnEnable();
        }

        public void OnComponentRemoved(Entity<TScope> entity, View component)
        {
            _entityViewDatabase.RemoveView(entity).ReturnToPool();
        }

        public void OnComponentReplaced(Entity<TScope> entity, View oldComponent, View newComponent)
        {
            OnComponentRemoved(entity, oldComponent);
            OnComponentAdded(entity, newComponent);
        }

        public void Destroy()
        {
            _contexts.Singleton().RemoveComponentListener(this);
        }

        private void UpdatePositionAndOrientation(Entity<TScope> entity)
        {
            _orientationHandler?.UpdateOrientation(entity);
            _positionHandler?.UpdatePostion(entity);
        }
    }
}