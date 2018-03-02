using Unscientificlab.ECS.Modules.Base;
using Unscientificlab.Util.Pool;

namespace Unscientificlab.ECS.Modules.Physics
{
    public class CollisionsCleanupSystem: ICleanupSystem
    {
        private readonly Context<Simulation> _simulation;

        public CollisionsCleanupSystem(Contexts contexts)
        {
            _simulation = contexts.Get<Simulation>();
        }

        public void Cleanup()
        {
            foreach (var entity in _simulation.AllWith<Collisions>())
            {
                var collisions = entity.Get<Collisions>().Value;

                foreach (var collision in collisions)
                {
                    collision.Return();
                }
                collisions.Clear();
                ListPool<Collision>.Instance.Return(collisions);
                entity.Remove<Collisions>();
            }
        }
    }
}