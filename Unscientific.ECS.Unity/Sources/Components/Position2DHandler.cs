using System;
using UnityEngine;
using Unscientific.ECS.Modules.Core;
using Unscientific.ECS.Modules.Physics2D;
using Unscientific.ECS.Modules.View;

namespace Unscientific.ECS.Unity
{
    public class PositionHandler<TScope>: MonoBehaviour, IHandler, IComponentListener<TScope, Position> where TScope : IScope
    {
        private Contexts _contexts;
        private ViewHandler<TScope> _viewHandler;
        private EntityViewDatabase<TScope> _entityViewDatabase;

        public void Initialize(Contexts contexts, MessageBus messageBus)
        {
            _contexts = contexts;
            _viewHandler = GetComponent<ViewHandler<TScope>>();
            _entityViewDatabase = GetComponent<EntityViewDatabase<TScope>>();
            _contexts.Singleton().AddComponentListener(this);
        }

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

        public void Destroy()
        {
            _contexts.Singleton().RemoveComponentListener(this);
        }

        private void MoveView(Entity<TScope> entity, Position position)
        {
            var view = _entityViewDatabase.GetView(entity);

            switch (_viewHandler.ViewPlane)
            {
                case ViewPlane.XY:
                    view.transform.localPosition = position.Value.ToVector3();
                    break;
                case ViewPlane.XZ:
                    view.transform.localPosition = position.Value.ToVector3XZ();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}