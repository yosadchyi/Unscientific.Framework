using System.Collections.Generic;
using Unscientific.ECS.Features.Core;

namespace Unscientific.ECS.Features.Tests.Sources.Core
{
    public class EmptyTestComponentNotificationsListener : IComponentListener<Game, EmptyTestComponent>
    {
        private readonly List<string> _log;

        public EmptyTestComponentNotificationsListener(List<string> log)
        {
            _log = log;
        }

        public void OnComponentAdded(Entity<Game> entity)
        {
            _log.Add("Added");
        }

        public void OnComponentRemoved(Entity<Game> entity, EmptyTestComponent component)
        {
            _log.Add("Removed");
        }

        public void OnComponentReplaced(Entity<Game> entity, EmptyTestComponent oldComponent)
        {
            _log.Add("Replaced");
        }
    }
}