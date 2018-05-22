using UnityEngine;
using Unscientific.ECS.Features.Core;
using Unscientific.ECS.Features.Destroy;
using Unscientific.ECS.Features.View;

namespace Unscientific.ECS.Unity
{
    public class DestroyHandler<TScope>: MonoBehaviour, IHandler, IComponentAddedListener<TScope, Destroyed>
    {
        private Contexts _contexts;

        public void Initialize(Contexts contexts, MessageBus messageBus)
        {
            _contexts = contexts;
            _contexts.Singleton().AddComponentAddedListener(this);
        }

        public void OnComponentAdded(Entity<TScope> entity)
        {
            entity.RemoveIfExists<View>();
        }

        public void Destroy()
        {
            _contexts.Singleton().RemoveComponentAddedListener(this);
        }
    }
}
