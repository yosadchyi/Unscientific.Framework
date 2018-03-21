using System.Collections.Generic;
using UnityEngine;

namespace Unscientific.ECS.Unity
{
    public class EntityViewDatabase<TScope> where TScope : IScope
    {
        private readonly Dictionary<int, GameObject> _entityToView = new Dictionary<int, GameObject>();

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
            var gameObject = GetView(entity);
            _entityToView.Remove(entity.Id);
            return gameObject;
        }
    }
}