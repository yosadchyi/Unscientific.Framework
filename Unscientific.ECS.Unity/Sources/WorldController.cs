using UnityEngine;

namespace Unscientific.ECS.Unity
{
    public class WorldController : MonoBehaviour
    {

        #region Exposed properties

        public World World { get; private set; }

        #endregion

        #region Internal state

        private Systems _systems;
        private MessageBus _messageBus;

        #endregion

        #region Unity methods

        private void Awake()
        {
            World = World.Instance;
            _systems = World.Systems;
            _messageBus = World.MessageBus;
        }

        private void Start()
        {
            _systems.Setup();
            foreach (var handler in gameObject.GetComponents<IHandler>())
            {
                handler.Initialize(World.Contexts, World.MessageBus);
            }
            System.GC.Collect();
        }

        private void FixedUpdate()
        {
            _systems.Update();
            _systems.Cleanup();
            _messageBus.Cleanup();
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
