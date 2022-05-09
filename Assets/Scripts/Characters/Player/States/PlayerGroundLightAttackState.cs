using Core.Attacking;
using Core.Characters.Interfaces;

namespace Characters.Player.States
{
    public class PlayerGroundLightAttackState : PlayerBaseGroundedState
    {
        private int _currentAttack;
        private bool _shouldContinueAttack;

        public override void EnterState()
        {
            Context.VelocityMovement.Stop();

            Context.AnimatorComponent.SetTrigger("attack");
            Context.Player.Stamina.Spend(Context.PlayerConfig.LightAttackFirstStaminaCost);

            _currentAttack = 1;
            _shouldContinueAttack = false;

            Context.FirstLightAttack.StartAttack(AttackEndedFirst);
        }

        private void AttackEndedFirst(AttackResult result)
        {
            if (result.WasCompleted && _shouldContinueAttack)
            {
                _currentAttack = 2;
                Context.Player.Stamina.Spend(Context.PlayerConfig.LightAttackSecondStaminaCost);
                Context.SecondLightAttack.StartAttack(AttackEndedSecond);
            }
            else
            {
                AttackEndedSecond(result);
            }
        }

        public override void UpdateState()
        {
            if (!_shouldContinueAttack && Context.Player.HasStamina() && Context.Input.IsLightAttackPressed)
            {
                Context.AnimatorComponent.SetTrigger("attack");
                _shouldContinueAttack = true;
            }
        }

        private void AttackEndedSecond(AttackResult result)
        {
            if (result.WasCompleted)
            {
                SwitchState<PlayerGroundedState>();
            }
            else
            {
                Context.AnimatorComponent.ResetTrigger("attack");
            }
        }

        private void InterruptCurrentAttack()
        {
            switch (_currentAttack)
            {
                case 1:
                    Context.FirstLightAttack.InterruptAttack();
                    break;
                case 2:
                    Context.SecondLightAttack.InterruptAttack();
                    break;
            }
        }

        public override void OnDeath(HitInfo info)
        {
            InterruptCurrentAttack();
            base.OnDeath(info);
        }

        public override void OnStaggered(HitInfo info)
        {
            InterruptCurrentAttack();
            base.OnStaggered(info);
        }
    }
}