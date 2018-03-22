using UnityEngine;
using Unscientific.ECS.Modules.Core;
using Unscientific.ECS.Modules.Physics;
using Unscientific.ECS.Modules.View;

namespace Unscientific.ECS.Unity
{
    public class OrientationHandler<TScope>: MonoBehaviour, IHandler, IComponentListener<TScope, Orientation> where TScope : IScope
    {
        private Contexts _contexts;
        private EntityViewDatabase<TScope> _entityViewDatabase;

        public void Initialize(Contexts contexts, MessageBus messageBus)
        {
            _contexts = contexts;
            _entityViewDatabase = GetComponent<EntityViewDatabase<TScope>>();
            _contexts.Singleton().AddComponentListener(this);
        }

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

        public void Destroy()
        {
            _contexts.Singleton().RemoveComponentListener(this);
        }

        private void RotateView(Entity<TScope> entity, Orientation orientation)
        {
            var view = _entityViewDatabase.GetView(entity);

            view.transform.eulerAngles = new Vector3(0, -orientation.Value.AsFloat * Mathf.Rad2Deg, 0);
        }
    }
}