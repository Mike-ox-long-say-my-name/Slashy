using Core.Attacking;

namespace Characters.Player.States
{
    public abstract class PlayerAirboneState : PlayerBaseState
    {
        public override void EnterState()
        {
            Context.Animator.SetBool("is-airbone", true);
        }

        public override void ExitState()
        {
            Context.Animator.SetBool("is-airbone", false);
        }

        public override void OnStaggered(HitInfo info)
        {
            base.OnStaggered(info);
            SwitchState<PlayerAirboneStaggerState>();
        }
    }
}