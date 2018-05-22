using Unscientific.FixedPoint;

namespace Unscientific.ECS.Features.Steering2D
{
    public struct SteeringVelocity
    {
        public static SteeringVelocity Zero = new SteeringVelocity (FixVec2.Zero);

        public FixVec2 Linear;
        public Fix Angular;

        public SteeringVelocity (FixVec2 linear, Fix angular)
        {
            Linear = linear;
            Angular = angular;
        }

        public SteeringVelocity (FixVec2 linear) : this (linear, Fix.Zero)
        {
        }

        public static SteeringVelocity operator + (SteeringVelocity a1, SteeringVelocity a2)
        {
            return new SteeringVelocity (a1.Linear + a2.Linear, a1.Angular + a2.Angular);
        }

        public static SteeringVelocity operator * (SteeringVelocity a, Fix scale)
        {
            return new SteeringVelocity (a.Linear * scale, a.Angular * scale);
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
