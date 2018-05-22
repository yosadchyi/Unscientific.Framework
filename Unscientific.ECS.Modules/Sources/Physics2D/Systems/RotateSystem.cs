using Unscientific.ECS.Modules.Core;
using Unscientific.ECS.Modules.Destroy;

namespace Unscientific.ECS.Modules.Physics2D
{
    public static class RotateSystem
    {
        public static void Update(Contexts contexts)
        {
            var dt = contexts.Singleton().Get<TimeStep>().Value;
            var context = contexts.Get<Game>();

            foreach (var entity in context.AllWith<Orientation, AngularVelocity>())
            {
                if (entity.Is<Destroyed>())
                    continue;

                var orientation = entity.Get<Orientation>().Value;
                var angularVelocity = entity.Get<AngularVelocity>().Value;

                entity.Replace(new Orientation(orientation + angularVelocity * dt));
            }
        }
    }
}