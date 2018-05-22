using System.Collections.Generic;
using UnityEngine;

namespace Unscientific.ECS.Unity
{
    public class EntityViewDatabase<TScope>: MonoBehaviour, IHandler
    {
        private readonly Dictionary<int, GameObject> _entityToView = new Dictionary<int, GameObject>();

        public void Initialize(Contexts contexts, MessageBus messageBus)
        {
        }

        public void AddView(Entity<TScope> entity, GameObject view)
        {
            _entityToView[entity.Id] = view;
        }

        public GameObject GetView(Entity<TScope> entity)
        {
            return _entityToView[entity.Id];
        }

        public GameObject RemoveView(Entity<TScope> entity)
        {
            var view = GetView(entity);
            _entityToView.Remove(entity.Id);
            return view;
        }

        public void Destroy()
        {
        }
    }
}