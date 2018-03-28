using Unscientific.ECS.Modules.Navigation;

namespace Unscientific.ECS.Modules.Steering
{
    public struct FlowField
    {
        public IFlowField Field;

        public FlowField(IFlowField field)
        {
            Field = field;
        }
    }
}