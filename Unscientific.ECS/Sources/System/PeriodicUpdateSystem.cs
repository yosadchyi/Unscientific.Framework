namespace Unscientific.ECS
{
    public class PeriodicUpdateSystem: IUpdateSystem
    {
        private readonly IUpdateSystem _decoratedSystem;
        private readonly int _period;
        private int _counter;

        public PeriodicUpdateSystem(IUpdateSystem decoratedSystem, int period)
        {
            _decoratedSystem = decoratedSystem;
            _period = period;
        }

        public void Update()
        {
            _counter++;
            if (_counter % _period == 0)
            {
                _decoratedSystem.Update();
                _counter = 0; // avoid overflow
            }
        }
    }
}