using System;
using System.Collections.Generic;

namespace Unscientific.ECS.Features.Steering2D
{
    public class CompositeBehaviourBuilder<TFinalizeResult> :
        SteeringBehaviourBuilderBase<CompositeBehaviourComponentFinalizer<CompositeBehaviourBuilder<TFinalizeResult>>, TFinalizeResult>
    {
        private readonly Func<TFinalizeResult> _getBuilderMethodResult;
        private readonly AcceptSteeringBehaviour _accept;
        private readonly List<SteeringBehaviour> _behaviours = new List<SteeringBehaviour>();
        private SteeringBehaviour _behaviour;

        public CompositeBehaviourBuilder(AcceptSteeringBehaviour accept, Func<TFinalizeResult> getBuilderMethodResult)
        {
            _accept = accept;
            _getBuilderMethodResult = getBuilderMethodResult;
        }

        protected override CompositeBehaviourComponentFinalizer<CompositeBehaviourBuilder<TFinalizeResult>> GetBuilderMethodResult()
        {
            return new CompositeBehaviourComponentFinalizer<CompositeBehaviourBuilder<TFinalizeResult>>(AddBehaviour, _behaviour, this);
        }

        protected override TFinalizeResult GetFinalizeResult()
        {
            return _getBuilderMethodResult();
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
            _accept(new CompositeBehaviour(_behaviours.ToArray()));
            return GetFinalizeResult();
        }
    }
}