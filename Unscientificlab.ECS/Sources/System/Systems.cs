using System.Collections.Generic;

namespace Unscientificlab.ECS
{
    public class Systems: IUpdateSystem
    {
        private readonly List<IUpdateSystem> _systems = new List<IUpdateSystem>();
        
        public Systems Add(IUpdateSystem system)
        {
            _systems.Add(system);

            return this;
        }

        public void Update()
        {
            foreach (var system in _systems)
            {
                system.Update();
            }
        }
    }
}
