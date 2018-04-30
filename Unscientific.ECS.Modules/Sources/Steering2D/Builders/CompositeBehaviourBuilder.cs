using System.Collections.Generic;

namespace Unscientific.ECS.Modules.Steering2D
{
    public class CompositeBehaviourBuilder<TFinalizeResult> :
        SteeringBehaviourBuilderBase<CompositeBehaviourComponentFinalizer<CompositeBehaviourBuilder<TFinalizeResult>>, TFinalizeResult>
    {
        private readonly TFinalizeResult _result;
        private readonly List<SteeringBehaviour> _behaviours = new List<SteeringBehaviour>();
        private SteeringBehaviour _behaviour;

        public CompositeBehaviourBuilder(AcceptSteeringBehaviour parentAccept, TFinalizeResult result) : base(parentAccept)
        {
            _result = result;
        }

        protected override CompositeBehaviourComponentFinalizer<CompositeBehaviourBuilder<TFinalizeResult>> GetBuilderMethodResult()
        {
            return new CompositeBehaviourComponentFinalizer<CompositeBehaviourBuilder<TFinalizeResult>>(AddBehaviour, _behaviour, this);
        }

        protected override TFinalizeResult GetFinalizeResult()
        {
            return _result;
        }

        private void AddBehaviour(SteeringBehaviour behaviour)
        {
            _behaviours.Add(behaviour);
        }

        protected override void Accept(SteeringBehaviour behaviour)
        {
            _behaviour = behaviour;
        }

        public TFinalizeResult End()
        {
            ParentAccept(new CompositeBehaviour(_behaviours.ToArray()));
            return _result;
        }
    }
}