using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using Unscientific.ECS.Modules.Core;
using Unscientific.ECS.Modules.Physics;
using Unscientific.ECS.Modules.View;
using Transform = UnityEngine.Transform;

namespace Unscientific.ECS.Unity
{
    public class ViewHandler<TScope>: IComponentListener<TScope, View> where TScope : IScope
    {
        private readonly Contexts _contexts;
        private readonly AssetFactory _assetFactory;
        private readonly EntityViewDatabase<TScope> _entityViewDatabase;

        public ViewHandler(Contexts contexts, AssetFactory assetFactory, EntityViewDatabase<TScope> entityViewDatabase)
        {
            _contexts = contexts;
            _assetFactory = assetFactory;
            _entityViewDatabase = entityViewDatabase;
            _contexts.Singleton().AddComponentListener(this);
        }

        public void Destroy()
        {
            _contexts.Singleton().RemoveComponentListener(this);
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
    }
}