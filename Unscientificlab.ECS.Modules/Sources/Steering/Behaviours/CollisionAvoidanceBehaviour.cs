namespace Unscientific.ECS.Modules.Steering
{
//    public class CollisionAvoidance : GroupSteeringBehaviour
//    {
//        private Entity<Simulation> _owner;
//        private Entity<Simulation> _firstNeighbor;
//        private Fix _firstMinSeparation;
//        private Fix _firstDistance;
//        private FixVec2 _relativePosition;
//        private FixVec2 _firstRelativePosition;
//        private FixVec2 _firstRelativeVelocity;
//        private FixVec2 _relativeVelocity;
//        private Fix _shortestTime;
//        private readonly Proximity.Callback _proximityCallback;
//
//        public CollisionAvoidance(Proximity proximity) : base(proximity)
//        {
//            _proximityCallback = Callback;
//        }
//
//        public override SteeringAcceleration DoCalculate(Entity<Simulation> owner)
//        {
//            var steering = SteeringAcceleration.Zero;
//
//            _owner = owner;
//            _shortestTime = Fix.MaxValue;
//            _firstNeighbor = null;
//            _firstMinSeparation = Fix.Zero;
//            _firstDistance = Fix.Zero;
//            _relativePosition = FixVec2.Zero;
//
//            var count = Proximity.FindNeighbors(owner, _proximityCallback);
//
//            if (count == 0 || _firstNeighbor == null)
//                return steering;
//
//            if (_firstMinSeparation <= 0 || _firstDistance < owner.BoundingRadius + _firstNeighbor.BoundingRadius)
//            {
//                _relativePosition = _firstNeighbor.Get<Position>().Value - owner.Get<Position>().Value;
//            }
//            else
//            {
//                _relativePosition = _firstRelativePosition - _firstRelativeVelocity * _shortestTime;
//            }
//
//            _relativePosition.Normalize();
//            var maxAcceleration = owner.Get<MaxAcceleration>().Value;
//            steering.Linear = _relativePosition * maxAcceleration;
//
//            return steering;
//        }
//
//        private bool Callback(Entity<Simulation> neighbor, Fix sqrRange)
//        {
//            _relativePosition = neighbor.Get<Position>().Value - _owner.Get<Position>().Value;
//            _relativeVelocity = neighbor.Get<Velocity>().Value - _owner.Get<Velocity>().Value;
//            var relativeVelocity2 = _relativeVelocity.MagnitudeSqr;
//
//            if (relativeVelocity2 == Fix.Zero)
//                return false;
//
//            var timeToCollision = -_relativePosition.Dot(_relativeVelocity) / relativeVelocity2;
//
//            if (timeToCollision <= Fix.Zero || timeToCollision >= _shortestTime)
//                return false;
//
//            var distance = _relativePosition.Magnitude;
//            var minSeparation = distance - FixMath.Sqrt(relativeVelocity2) * timeToCollision;
//
//            if (minSeparation > _owner.BoundingRadius + neighbor.BoundingRadius)
//                return false;
//
//            _shortestTime = timeToCollision;
//            _firstNeighbor = neighbor;
//            _firstMinSeparation = minSeparation;
//            _firstDistance = distance;
//            _firstRelativePosition = _relativePosition;
//            _firstRelativeVelocity = _relativeVelocity;
//
//            return true;
//        }
//    }
}