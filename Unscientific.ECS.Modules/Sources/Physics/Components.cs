using System.Collections.Generic;
using Unscientific.ECS.Modules.Physics.Shapes;
using Unscientific.FixedPoint;

namespace Unscientific.ECS.Modules.Physics
{
    public struct TimeStep
    {
        public readonly Fix Value;

        public TimeStep(Fix value)
        {
            Value = value;
        }
    }
    
    public struct Position
    {
        public readonly FixVec2 Value;

        public Position(Fix x, Fix y)
        {
            Value = new FixVec2(x, y);
        }
        
        public Position(FixVec2 value)
        {
            Value = value;
        }
    }

    public struct Velocity
    {
        public readonly FixVec2 Value;

        public Velocity(Fix x, Fix y)
        {
            Value = new FixVec2(x, y);
        }
        
        public Velocity(FixVec2 value)
        {
            Value = value;
        }
    }

    public struct Damping
    {
        public readonly Fix Value;

        public Damping(Fix value)
        {
            Value = value;
        }
    }

    public struct Force
    {
        public readonly FixVec2 Value;

        public Force(Fix x, Fix y)
        {
            Value = new FixVec2(x, y);
        }

        public Force(FixVec2 value)
        {
            Value = value;
        }
    }

    public struct MaxVelocity
    {
        public readonly Fix Value;

        public MaxVelocity(Fix value)
        {
            Value = value;
        }
    }

    public struct Mass
    {
        public readonly Fix Value;

        public Mass(Fix value)
        {
            Value = value;
        }
    }

    public struct Orientation
    {
        public readonly Fix Value;

        public Orientation(Fix value)
        {
            Value = value;
        }
    }

    public struct AngularVelocity
    {
        public readonly Fix Value;

        public AngularVelocity(Fix value)
        {
            Value = value;
        }
    }

    public struct MaxAngularVelocity
    {
        public readonly Fix Value;

        public MaxAngularVelocity(Fix value)
        {
            Value = value;
        }
    }

    public struct AngularDamping
    {
        public readonly Fix Value;

        public AngularDamping(Fix value)
        {
            Value = value;
        }
    }

    public struct Torque
    {
        public readonly Fix Value;

        public Torque(Fix value)
        {
            Value = value;
        }
    }

    public struct Inertia
    {
        public readonly Fix Value;

        public Inertia(Fix value)
        {
            Value = value;
        }
    }

    public struct BoundingShape
    {
        public readonly Shape Shape;

        public BoundingShape(Shape shape)
        {
            Shape = shape;
        }
    }

    public struct Collisions
    {
        public readonly List<Collision> List;

        public Collisions(List<Collision> list)
        {
            List = list;
        }
    }
}
