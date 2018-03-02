using Unscientificlab.ECS.Modules.Base;

namespace Unscientificlab.ECS.Modules.Physics
{
    public class ShapesCleanupSystem: ICleanupSystem
    {
        private readonly Context<Simulation> _simulation;

        public ShapesCleanupSystem(Contexts contexts)
        {
            _simulation = contexts.Get<Simulation>();
        }

        public void Cleanup()
        {
            foreach (var entity in _simulation.AllWith<Destroyed, BoundingShape>())
            {
                entity.Get<BoundingShape>().Shape.Return();
                entity.Remove<BoundingShape>();
            }            
        }
    }
}