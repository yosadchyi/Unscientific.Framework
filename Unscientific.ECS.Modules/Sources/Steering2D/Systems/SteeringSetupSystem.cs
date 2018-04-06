using Unscientific.ECS.Modules.Core;

namespace Unscientific.ECS.Modules.Steering2D
{
    public class SteeringSetupSystem: ISetupSystem
    {
        private readonly Context<Configuration> _configuration;
        private readonly int _updatePeriod;

        public SteeringSetupSystem(Contexts contexts, int updatePeriod)
        {
            _updatePeriod = updatePeriod;
            _configuration = contexts.Get<Configuration>();
        }

        public void Setup()
        {
            _configuration.Singleton().Add(new SteeringUpdatePeriod(_updatePeriod));
        }
    }
}