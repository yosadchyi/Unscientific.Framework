namespace Unscientificlab.Util
{
    public class ArithmeticUtils
    {
        public static int ClosestPowerOfTwoGreaterThan(int x)
        {
            x--;
            x |= (x >> 1);
            x |= (x >> 2);
            x |= (x >> 4);
            x |= (x >> 8);
            x |= (x >> 16);
            return (x + 1);
        }
    }
}

