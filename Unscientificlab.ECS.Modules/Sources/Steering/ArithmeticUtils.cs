using Unscientific.FixedPoint;

namespace Unscientific.ECS.Modules.Steering
{
    public class ArithmeticUtils
    {
        public static Fix WrapAngleAroundZero (Fix a)
        {
            if (a >= 0) {
                var rotation = a % FixMath.Pi2;
                while (rotation > FixMath.Pi) rotation -= FixMath.Pi2;
                return rotation;
            } else {
                var rotation = -a % FixMath.Pi2;
                while (rotation > FixMath.Pi) rotation -= FixMath.Pi2;
                return -rotation;
            }
        }
    }
}

