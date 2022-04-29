using Core.Attacking;
using Core.Characters;

namespace Characters.Player.States
{
    public class PlayerGroundStrongAttackState : PlayerBaseState
    {
        private int _currentAttack;
        private bool _shouldContinueAttack;

        public override void EnterState()
        {
            var selfHit = new HitInfo
            {
                DamageStats = new DamageStats
                {
                    BaseDamage = Context.PlayerConfig.FirstStrongAttackHealthCost,
                    BaseBalanceDamage = 0
                },
                Multipliers = DamageMultipliers.One,
                Source = HitSource.AsCharacter(Context.Character)
            };

            Context.VelocityMovement.Stop();

            Context.AnimatorComponent.SetTrigger("strong-attack");
            Context.Character.SpendStamina(Context.PlayerConfig.FirstStrongAttackStaminaCost);

            _currentAttack = 1;
            _shouldContinueAttack = false;

            Context.FirstStrongAttack.StartAttack(AttackEndedFirst);
            Context.Character.ReceiveHit(selfHit);
        }

        private void AttackEndedFirst(bool interrupted)
        {
            if (!interrupted && _shouldContinueAttack)
            {
                _currentAttack = 2;
                Context.Character.SpendStamina(Context.PlayerConfig.SecondStrongAttackStaminaCost);
                Context.SecondStrongAttack.StartAttack(AttackEndedSecond);
            }
            else
            {
                AttackEndedSecond(interrupted);
            }
        }

        public override void UpdateState()
        {
            if (!_shouldContinueAttack && Context.Character.HasStamina() && Context.Input.IsStrongAttackPressed)
            {
                Context.AnimatorComponent.SetTrigger("strong-attack");
                _shouldContinueAttack = true;
            }
        }

        private void AttackEndedSecond(bool interrupted)
        {
            if (!interrupted)
            {
                SwitchState<PlayerGroundedState>();
            }
            else
            {
                Context.AnimatorComponent.ResetTrigger("strong-attack");
            }
        }

        private void InterruptCurrentAttack()
        {
            switch (_currentAttack)
            {
                case 1:
                    if (Context.FirstStrongAttack.IsAttacking)
                    {
                        Context.FirstStrongAttack.InterruptAttack();
                    }
                    break;
                case 2:
                    if (Context.SecondStrongAttack.IsAttacking)
                    {
                        Context.SecondStrongAttack.InterruptAttack();
                    }
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