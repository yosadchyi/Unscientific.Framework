namespace Unscientific.ECS
{
    public class PeriodicUpdateSystem: IUpdateSystem
    {
        private IUpdateSystem _decordatedSystem;
        private readonly int _period;
        private int _counter;

        public PeriodicUpdateSystem(IUpdateSystem decordatedSystem, int period)
        {
            _decordatedSystem = decordatedSystem;
            _period = period;
        }

        public void Update()
        {
            _counter++;
            if (_counter % _period == 0)
            {
                _decordatedSystem.Update();
                _counter = 0; // avoid overflow
            }
        }
    }
}