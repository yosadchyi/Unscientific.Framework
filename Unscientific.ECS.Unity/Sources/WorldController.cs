using UnityEngine;

namespace Unscientific.ECS.Unity
{
    public class WorldController : MonoBehaviour
    {

        #region Exposed properties

        public World World { get; private set; }

        #endregion

        #region Unity methods

        private void Awake()
        {
            World = World.Instance;
        }

        private void Start()
        {
            World.Setup();
            foreach (var handler in gameObject.GetComponents<IHandler>())
            {
                handler.Initialize(World.Contexts, World.MessageBus);
            }
            System.GC.Collect();
        }

        private void FixedUpdate()
        {
            World.Update();
            World.Cleanup();
        }

        private void OnDestroy()
        {
            foreach (var handler in gameObject.GetComponents<IHandler>())
            {
                handler.Destroy();
            }
            World.Clear();
        }

        #endregion
    }
}
