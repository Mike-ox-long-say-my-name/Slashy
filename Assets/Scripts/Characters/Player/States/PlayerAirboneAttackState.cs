using Core.Attacking;

namespace Characters.Player.States
{
    public class PlayerAirboneAttackState : PlayerAirboneState
    {
        public PlayerAirboneAttackState(PlayerStateMachine context, PlayerStateFactory factory) : base(context, factory)
        {
        }

        public override void EnterState()
        {
            base.EnterState();
            Context.AttackedAtThisAirTime = true;

            Context.AnimatorComponent.SetTrigger("attack");
            Context.Movement.ResetGravity();
            Context.LightAirboneAttack.StartAttack(OnAttackEnded);
        }

        private void OnAttackEnded(bool _)
        {
            if (Context.Movement.IsGrounded)
            {
                SwitchState(Factory.Grounded());
            }
            else
            {
                SwitchState(Factory.Fall());
            }
        }

        public override void OnStaggered(HitInfo info)
        {
            Context.LightAirboneAttack.InterruptAttack();
            base.OnStaggered(info);
        }

        public override void OnDeath(HitInfo info)
        {
            Context.LightAirboneAttack.InterruptAttack();
            base.OnDeath(info);
        }
    }
}