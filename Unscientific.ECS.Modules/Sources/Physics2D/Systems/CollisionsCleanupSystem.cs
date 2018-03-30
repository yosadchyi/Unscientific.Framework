using Unscientific.ECS.Modules.Core;

namespace Unscientific.ECS.Modules.Physics2D
{
    public class CollisionsCleanupSystem: ICleanupSystem
    {
        private readonly Context<Game> _context;

        public CollisionsCleanupSystem(Contexts contexts)
        {
            _context = contexts.Get<Game>();
        }

        public void Cleanup()
        {
            foreach (var entity in _context.AllWith<Collisions>())
            {
                entity.Get<Collisions>().List.Clear();
            }
        }
    }
}