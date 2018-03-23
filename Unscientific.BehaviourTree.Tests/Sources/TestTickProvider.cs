namespace Unscientific.BehaviourTree.Tests
{
    public class TestTickProvider: ITickProvider
    {
        private int _tick;

        public int GetTick()
        {
            return _tick;
        }

        public void Tick()
        {
            _tick++;
        }
    }
}