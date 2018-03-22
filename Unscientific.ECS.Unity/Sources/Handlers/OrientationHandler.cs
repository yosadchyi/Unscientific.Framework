using UnityEngine;
using Unscientific.ECS.Modules.Core;
using Unscientific.ECS.Modules.Physics;
using Unscientific.ECS.Modules.View;

namespace Unscientific.ECS.Unity
{
    public class OrientationHandler<TScope>: IComponentListener<TScope, Orientation> where TScope : IScope
    {
        private readonly Contexts _contexts;
        private readonly EntityViewDatabase<TScope> _entityViewDatabase;

        public OrientationHandler(Contexts contexts, EntityViewDatabase<TScope> entityViewDatabase)
        {
            _contexts = contexts;
            _entityViewDatabase = entityViewDatabase;
            _contexts.Singleton().AddComponentListener(this);
        }

        public void Destroy()
        {
            _contexts.Singleton().RemoveComponentListener(this);
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

        private void RotateView(Entity<TScope> entity, Orientation orientation)
        {
            var view = _entityViewDatabase.GetView(entity);

            view.transform.eulerAngles = new Vector3(0, -orientation.Value.AsFloat * Mathf.Rad2Deg, 0);
        }
    }
}