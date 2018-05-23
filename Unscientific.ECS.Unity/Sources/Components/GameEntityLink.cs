using Unscientific.ECS.Features.Core;

namespace Unscientific.ECS.Unity
{
    public class GameEntityLink: EntityLink<Game>
    {
        public BoxedEntity BoxedEntity => Entity.Box();
    }
}