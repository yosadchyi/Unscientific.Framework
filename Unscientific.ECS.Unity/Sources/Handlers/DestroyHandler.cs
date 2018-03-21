using Unscientific.ECS.Modules.Core;
using Unscientific.ECS.Modules.View;

namespace Unscientific.ECS.Unity
{
    public class DestroyHandler<TScope>: IComponentAddedListener<TScope, Destroyed> where TScope : IScope
    {
        private readonly Contexts _contexts;

        public DestroyHandler(Contexts contexts)
        {
            _contexts = contexts;
            _contexts.Singleton().AddComponentAddedListener(this);
        }

        public void OnComponentAdded(Entity<TScope> entity, Destroyed destroyed)
        {
            if (entity.Has<View>()) entity.Remove<View>();
        }

        public void Destroy()
        {
            _contexts.Singleton().RemoveComponentAddedListener(this);
        }
    }
}