using Unscientific.ECS.Modules.Core;

namespace Unscientific.ECS.Modules.Physics
{
    public class ShapesCleanupSystem: ICleanupSystem
    {
        private readonly Context<Game> _context;

        public ShapesCleanupSystem(Contexts contexts)
        {
            _context = contexts.Get<Game>();
        }

        public void Cleanup()
        {
            foreach (var entity in _context.AllWith<Destroyed, BoundingShape>())
            {
                entity.Get<BoundingShape>().Shape.Return();
                entity.Remove<BoundingShape>();
            }
        }
    }
}