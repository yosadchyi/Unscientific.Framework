namespace Unscientific.BehaviourTree.Tests
{
    public class TestTickSupplier: ITickSupplier
    {
        private int _tick;

        public int Supply()
        {
            return _tick;
        }

        public void Tick()
        {
            _tick++;
        }
    }
}