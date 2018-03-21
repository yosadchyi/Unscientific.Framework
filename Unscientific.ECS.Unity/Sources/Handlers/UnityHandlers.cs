using UnityEngine;
using Unscientific.ECS.Modules.Core;

namespace Unscientific.ECS.Unity
{
    public class UnityHandlers: MonoBehaviour
    {
        public AssetFactory AssetFactory;
        public EntityViewDatabase<Game> EntityViewDatabase;
        public DestroyHandler<Game> DestroyHandler;
        public ViewHandler<Game> ViewHandler;
        public PositionHandler<Game> PositionHandler;
        public OrientationHandler<Game> OrientationHandler;

        public void Initialize(Contexts contexts, Transform parenTransform)
        {
            AssetFactory = new AssetFactory(parenTransform);
            EntityViewDatabase = new EntityViewDatabase<Game>();
            DestroyHandler = new DestroyHandler<Game>(contexts);
            ViewHandler = new ViewHandler<Game>(contexts, AssetFactory, EntityViewDatabase);
            PositionHandler = new PositionHandler<Game>(contexts, EntityViewDatabase);
            OrientationHandler = new OrientationHandler<Game>(contexts, EntityViewDatabase);
        }

        public void Destroy()
        {
            DestroyHandler.Destroy();
            ViewHandler.Destroy();
            PositionHandler.Destroy();
            OrientationHandler.Destroy();

            AssetFactory = null;
            EntityViewDatabase = null;
            DestroyHandler = null;
            ViewHandler = null;
            PositionHandler = null;
            OrientationHandler = null;
        }
    }
}