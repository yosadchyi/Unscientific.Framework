﻿using UnityEngine;
using Unscientific.ECS.Modules.Core;
using Unscientific.ECS.Modules.View;

namespace Unscientific.ECS.Unity
{
    public class DestroyHandler<TScope>: MonoBehaviour, IHandler, IComponentAddedListener<TScope, Destroyed> where TScope : IScope
    {
        private Contexts _contexts;

        public void Initialize(Contexts contexts, MessageBus messageBus)
        {
            _contexts = contexts;
            _contexts.Singleton().AddComponentAddedListener(this);
        }

        public void OnComponentAdded(Entity<TScope> entity, Destroyed destroyed)
        {
            entity.RemoveIfExists<View>();
        }

        public void Destroy()
        {
            _contexts.Singleton().RemoveComponentAddedListener(this);
        }
    }
}
