using Core.Attacking;

namespace Characters.Player.States
{
    public abstract class PlayerAirboneState : PlayerBaseState
    {
        public override void EnterState()
        {
            Context.Animator.StartAirboneAnimation();
        }

        public override void ExitState()
        {
            Context.Animator.EndAirboneAnimation();
        }

        public override void OnStaggered(HitInfo info)
        {
            base.OnStaggered(info);
            SwitchState<PlayerAirboneStaggerState>(true);
        }
    }
}