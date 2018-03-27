using Unscientificlab.FixedPoint;

namespace Unscientificlab.Steering
{
    public struct Steering
    {
        public static Steering Zero = new Steering (FixVec2.Zero);

        public FixVec2 Linear;
        public Fix Angular;

        public Steering (FixVec2 linear, Fix angular)
        {
            Linear = linear;
            Angular = angular;
        }

        public Steering (FixVec2 linear) : this (linear, Fix.Zero)
        {
        }

        public static Steering operator + (Steering a1, Steering a2)
        {
            return new Steering (a1.Linear + a2.Linear, a1.Angular + a2.Angular);
        }

        public static Steering operator * (Steering a, Fix scale)
        {
            return new Steering (a.Linear * scale, a.Angular * scale);
        }

        public bool IsZero ()
        {
            return Linear == FixVec2.Zero && Angular == Fix.Zero;
        }

        public void Limit(Fix maxAcceleration, Fix maxAngularAcceleration)
        {
            FixVec2.ClampMagnitude(ref Linear, maxAcceleration);

            if (Angular > maxAngularAcceleration)
                Angular = maxAngularAcceleration;
        }
    }
}
