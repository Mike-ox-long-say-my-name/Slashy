using Core.Attacking;

namespace Characters.Player.States
{
    public abstract class PlayerBaseGroundedState : PlayerBaseState
    {
        public override void OnStaggered(HitInfo info)
        {
            base.OnStaggered(info);
            SwitchState<PlayerGroundStaggerState>();
        }
    }
}