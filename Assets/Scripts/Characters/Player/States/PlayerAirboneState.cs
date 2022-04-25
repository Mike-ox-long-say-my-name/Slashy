using Core.Attacking;

namespace Characters.Player.States
{
    public abstract class PlayerAirboneState : PlayerBaseState
    {
        protected PlayerAirboneState(PlayerStateMachine context, PlayerStateFactory factory) : base(context, factory)
        {
        }

        public override void EnterState()
        {
            Context.AnimatorComponent.SetBool("is-airbone", true);
        }

        public override void ExitState()
        {
            Context.AnimatorComponent.SetBool("is-airbone", false);
        }

        public override void OnStaggered(HitInfo info)
        {
            SwitchState(Factory.AirboneStagger());
        }
    }
}