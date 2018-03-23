namespace Unscientific.ECS.Tests
{
    public struct TestMessage
    {
        public readonly int Value;

        public TestMessage(int value)
        {
            Value = value;
        }
    }

    public struct AggregatedTestMessage
    {
        public readonly int Value;

        public AggregatedTestMessage(int value)
        {
            Value = value;
        }
    }

    public struct DelayedTestMessage
    {
        public readonly int Value;

        public DelayedTestMessage(int value)
        {
            Value = value;
        }
    }
}