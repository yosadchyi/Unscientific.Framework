using System;
using UnityEngine;
using Unscientific.ECS.Features.Core;
using Unscientific.ECS.Features.Physics2D;
using Unscientific.ECS.Features.View;

namespace Unscientific.ECS.Unity
{
    public class Orientation2DHandler<TScope>: MonoBehaviour, IOrientationHandler<TScope>, IComponentListener<TScope, Orientation>
    {
        private Contexts _contexts;
        private ViewHandler<TScope> _viewHandler;
        private EntityViewDatabase<TScope> _entityViewDatabase;

        public void Initialize(Contexts contexts, MessageBus messageBus)
        {
            _contexts = contexts;
            _entityViewDatabase = GetComponent<EntityViewDatabase<TScope>>();
            _viewHandler = GetComponent<ViewHandler<TScope>>();
            _contexts.Singleton().AddComponentListener(this);
        }

        public void OnComponentAdded(Entity<TScope> entity)
        {
            UpdateOrientation(entity);
        }

        public void OnComponentRemoved(Entity<TScope> entity, Orientation orientation)
        {
        }

        public void OnComponentReplaced(Entity<TScope> entity, Orientation oldOrientation)
        {
            UpdateOrientation(entity);
        }

        public void Destroy()
        {
            _contexts.Singleton().RemoveComponentListener(this);
        }

        public void UpdateOrientation(Entity<TScope> entity)
        {
            if (entity.Has<View>()) RotateView(entity, entity.Get<Orientation>());
        }

        private void RotateView(Entity<TScope> entity, Orientation orientation)
        {
            var view = _entityViewDatabase.GetView(entity);

            switch (_viewHandler.ViewPlane)
            {
                case ViewPlane.XY:
                    view.transform.eulerAngles = new Vector3(0, 0, -orientation.Value.AsFloat * Mathf.Rad2Deg);
                    break;
                case ViewPlane.XZ:
                    view.transform.eulerAngles = new Vector3(0, -orientation.Value.AsFloat * Mathf.Rad2Deg, 0);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}