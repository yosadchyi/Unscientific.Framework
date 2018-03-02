using Unscientificlab.ECS.Modules.Base;
using Unscientificlab.FixedPoint;
using Unscientificlab.ECS.Modules.Navigation;

namespace Unscientificlab.ECS.Modules.Steering
{
    public struct Steering
    {
        public readonly SteeringBehaviour SteeringBehaviour;

        public Steering(SteeringBehaviour steeringBehaviour)
        {
            SteeringBehaviour = steeringBehaviour;
        }
    }
    
    public struct TargetEntity
    {
        public int EntityId;

        public TargetEntity(int id)
        {
            EntityId = id;
        }

        public TargetEntity(Entity<Simulation> entity)
        {
            EntityId = entity.Id;
        }
    }

    public struct TargetPosition
    {
        public FixVec2 Value;

        public TargetPosition(FixVec2 value)
        {
            Value = value;
        }
    }

    public struct FlowField
    {
        public IFlowField Field;

        public FlowField(IFlowField field)
        {
            Field = field;
        }
    }

    public struct TargetOrientation
    {
        public Fix Value;

        public TargetOrientation(Fix value)
        {
            Value = value;
        }
    }

    public struct ArrivalTolerance
    {
        public Fix Distance;
        public Fix DecelerationDistance;

        public ArrivalTolerance(Fix distance, Fix decelerationDistance)
        {
            Distance = distance;
            DecelerationDistance = decelerationDistance;
        }
    }

    public struct AlignTolerance
    {
        public Fix Angle;
        public Fix DecelerationAngle;

        public AlignTolerance(Fix angle, Fix decelerationAngle)
        {
            Angle = angle;
            DecelerationAngle = decelerationAngle;
        }
    }
}