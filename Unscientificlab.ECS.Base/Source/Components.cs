namespace Unscientificlab.ECS.Base
{
    /// <summary>
    /// Component which indicates that entity is destroyed and should not be touched. 
    /// </summary>
    public struct Destroyed
    {
    }

    public struct Tick
    {
        public int Value;

        public Tick(int value)
        {
            Value = value;
        }
    }
}