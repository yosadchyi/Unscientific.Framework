using Unscientific.ECS.Modules.Core;
using Unscientific.ECS.Modules.Physics;
using Unscientific.ECS.Modules.View;

namespace Unscientific.ECS.Unity
{
    public class PositionHandler<TScope>: IComponentListener<TScope, Position> where TScope : IScope
    {
        private readonly Contexts _contexts;
        private readonly EntityViewDatabase<TScope> _entityViewDatabase;

        public PositionHandler(Contexts contexts, EntityViewDatabase<TScope> entityViewDatabase)
        {
            _contexts = contexts;
            _entityViewDatabase = entityViewDatabase;
            _contexts.Singleton().AddComponentListener(this);
        }

        public void Destroy()
        {
            _contexts.Singleton().RemoveComponentListener(this);
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

        private void MoveView(Entity<TScope> entity, Position position)
        {
            var view = _entityViewDatabase.GetView(entity);

            view.transform.localPosition = position.Value.ToVector3();
        }
    }
}