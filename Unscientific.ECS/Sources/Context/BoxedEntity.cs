using System;
using System.Collections.Generic;

namespace Unscientific.ECS
{
    public class BoxedEntity
    {
        private readonly int _id;
        private readonly ComponentInfo[] _componentsInfo;
        private readonly List<object> _componentCache = new List<object>();

        public object[] Components => GetComponents();
        
        internal BoxedEntity(int id, ComponentInfo[] componentsInfo)
        {
            _id = id;
            _componentsInfo = componentsInfo;
        }

        public object[] GetComponents()
        {
            _componentCache.Clear();

            foreach (var componentInfo in _componentsInfo)
            {
                if (componentInfo.HasComponent(_id))
                {
                    _componentCache.Add(componentInfo.GetComponent(_id));
                }
            }

            return _componentCache.ToArray();
        }
        
        public object GetComponent(Type componentType)
        {
            var info = GetComponentInfoForType(componentType);

            return info.GetComponent(_id);
        }

        public void RemoveComponent(Type componentType)
        {
            var info = GetComponentInfoForType(componentType);

            info.RemoveComponent(_id);
        }

        public void AddComponent(Type componentType, object component)
        {
            var info = GetComponentInfoForType(componentType);

            info.AddComponent(_id, component);
        }

        public void ReplaceComponent(Type componentType, object component)
        {
            var info = GetComponentInfoForType(componentType);

            info.ReplaceComponent(_id, component);
        }

        private ComponentInfo GetComponentInfoForType(Type componentType)
        {
            foreach (var componentInfo in _componentsInfo)
            {
                if (componentInfo.Type == componentType)
                {
                    return componentInfo;
                }
            }

            throw new ArgumentException($"Component type `{componentType.Name}' is not registered!");
        }
    }
}