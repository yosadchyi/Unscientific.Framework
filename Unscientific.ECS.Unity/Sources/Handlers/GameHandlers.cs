using UnityEngine;
using Unscientific.ECS.Modules.Core;

namespace Unscientific.ECS.Unity
{
    public class GameEntityViewDatabase : EntityViewDatabase<Game>
    {
    }

    public class GameDestroyHandler : DestroyHandler<Game>
    {
    }

    [RequireComponent(typeof(AssetFactory), typeof(GameEntityViewDatabase))]
    public class GameViewHandler : ViewHandler<Game>
    {
    }
    
    [RequireComponent(typeof(GameEntityViewDatabase))]
    public class GamePositionHandler : PositionHandler<Game>
    {
    }
    
    [RequireComponent(typeof(GameEntityViewDatabase))]
    public class GameOrientationHandler : OrientationHandler<Game>
    {
    }
}