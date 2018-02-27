using System.Collections.Generic;

namespace Unscientificlab.ECS
{
    public class Systems: ISystem
    {
        private readonly List<ISystem> _systems = new List<ISystem>();
        
        public Systems Add(ISystem system)
        {
            _systems.Add(system);

            return this;
        }

        public void Execute()
        {
            foreach (var system in _systems)
            {
                system.Execute();
            }
        }
    }
}
