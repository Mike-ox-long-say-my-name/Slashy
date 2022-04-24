using Core.Attacking;

namespace Characters.Player.States
{
    public abstract class PlayerBaseGroundedState : PlayerBaseState
    {
        protected PlayerBaseGroundedState(PlayerStateMachine context, PlayerStateFactory factory) : base(context, factory)
        {
        }

        public override void OnStaggered(HitInfo info)
        {
            SwitchState(Factory.GroundStagger());
        }
    }
}