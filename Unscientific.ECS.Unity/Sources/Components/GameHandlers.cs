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

    [RequireComponent(typeof(AssetFactory), typeof(GameEntityViewDatabase), typeof(GameDestroyHandler))]
    public class GameViewHandler : ViewHandler<Game>
    {
    }
    
    [RequireComponent(typeof(GameEntityViewDatabase))]
    public class GamePosition2DHandler : Position2DHandler<Game>
    {
    }
    
    [RequireComponent(typeof(GameEntityViewDatabase))]
    public class GameOrientation2DHandler : Orientation2DHandler<Game>
    {
    }
}