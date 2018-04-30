using Unscientific.FixedPoint;

namespace Unscientific.ECS.Modules.Steering2D
{
    public abstract class SteeringBehaviourBuilderBase<TBuilderMethodResult, TFinalizeResult>
    {
        protected readonly AcceptSteeringBehaviour ParentAccept;

        protected SteeringBehaviourBuilderBase(AcceptSteeringBehaviour parentAccept)
        {
            ParentAccept = parentAccept;
        }

        public TBuilderMethodResult ArriveToTarget()
        {
            var behaviour = new ArriveToTarget();
            ParentAccept(behaviour);
            return GetBuilderMethodResult();
        }
        
        public TBuilderMethodResult AlignVelocityWithNeighbors(Proximity proximity)
        {
            var behaviour = new AlignVelocityWithNeighbors(proximity);
            ParentAccept(behaviour);
            return GetBuilderMethodResult();
        }

        public TBuilderMethodResult CoheseWithNeighbors(Proximity proximity)
        {
            var behaviour = new CoheseWithNeighbors(proximity);
            ParentAccept(behaviour);
            return GetBuilderMethodResult();
        }

        public TBuilderMethodResult FollowFlowField()
        {
            var behaviour = new FollowFlowField();
            ParentAccept(behaviour);
            return GetBuilderMethodResult();
        }
        
        public TBuilderMethodResult LookAccordingToFlowField()
        {
            var behaviour = new LookAccordingToFlowField();
            ParentAccept(behaviour);
            return GetBuilderMethodResult();
        }

        public TBuilderMethodResult LootAtTarget()
        {
            var behaviour = new LookAtTarget();
            ParentAccept(behaviour);
            return GetBuilderMethodResult();
        }
        
        public TBuilderMethodResult LookWhereYouAreGoing()
        {
            var behaviour = new LookWhereYouAreGoing();
            ParentAccept(behaviour);
            return GetBuilderMethodResult();
        }
        
        public TBuilderMethodResult ReachTargetOrientation()
        {
            var behaviour = new ReachTargetOrientation();
            ParentAccept(behaviour);
            return GetBuilderMethodResult();
        }
        
        public TBuilderMethodResult SeekTarget()
        {
            var behaviour = new SeekTarget();
            ParentAccept(behaviour);
            return GetBuilderMethodResult();
        }
        
        public TBuilderMethodResult SeparateFromNeighbors(Proximity proximity)
        {
            var behaviour = new SeparateFromNeighbors(proximity);
            ParentAccept(behaviour);
            return GetBuilderMethodResult();
        }

        public WeightedBehaviourBuilder<TFinalizeResult> Weighted(Fix weight)
        {
            return new WeightedBehaviourBuilder<TFinalizeResult>(Accept, GetFinalizeResult(), weight);
        }

        public CompositeBehaviourBuilder<TBuilderMethodResult> Compose()
        {
            return new CompositeBehaviourBuilder<TBuilderMethodResult>(Accept, GetBuilderMethodResult());
        }

        protected abstract TBuilderMethodResult GetBuilderMethodResult();
        protected abstract TFinalizeResult GetFinalizeResult();
        protected abstract void Accept(SteeringBehaviour behaviour);
    }
}