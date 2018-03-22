using UnityEngine;

namespace Unscientific.ECS.Unity
{
    [RequireComponent(typeof(Handlers))]
    public class WorldController : MonoBehaviour
    {
        #region Exposed properties

        public World World { get; private set; }

        #endregion

        #region Internal state

        private Handlers _handlers;
        private Systems _systems;
        private MessageBus _messageBus;

        #endregion

        #region Unity methods

        private void Awake()
        {
            World = World.Instance;
            _handlers = GetComponent<Handlers>();
            _systems = World.Systems;
            _messageBus = World.MessageBus;
        }

        private void Start()
        {
            _systems.Setup();
            _handlers.Initialize(World.Contexts, World.MessageBus);
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
            _handlers.Destroy();
            World.Clear();
        }

        #endregion
    }
}
