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

        public void Initialize(Contexts contexts, MessageBus messageBus)
        {
            _contexts = contexts;
            _assetFactory = GetComponent<AssetFactory>();
            _entityViewDatabase = GetComponent<EntityViewDatabase<TScope>>();
            _contexts.Singleton().AddComponentListener(this);
        }

        public void OnComponentAdded(Entity<TScope> entity, View view)
        {
            _entityViewDatabase.AddView(entity, _assetFactory.CreateAsset(view.Name));
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
    }
}