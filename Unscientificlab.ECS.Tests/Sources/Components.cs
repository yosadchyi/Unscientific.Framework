namespace Unscientificlab.ECS.Tests
{
    public struct DeadFlagComponent
    {
    }
    
    public struct ValueComponent
    {
        public readonly int Value;

        public ValueComponent(int value)
        {
            Value = value;
        }
    }
}