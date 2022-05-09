using Core.Attacking;
using Core.Characters;
using Core.Characters.Interfaces;

namespace Characters.Player.States
{
    public class PlayerGroundStrongAttackState : PlayerBaseGroundedState
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
                Source = HitSource.AsCharacter(Context.Player.Character)
            };

            Context.VelocityMovement.Stop();

            Context.AnimatorComponent.SetTrigger("strong-attack");
            Context.Player.Stamina.Spend(Context.PlayerConfig.FirstStrongAttackStaminaCost);

            _currentAttack = 1;
            _shouldContinueAttack = false;

            Context.FirstStrongAttack.StartAttack(AttackEndedFirst);
            Context.HitReceiver.ReceiveHit(selfHit);
        }

        private void AttackEndedFirst(AttackResult result)
        {
            if (result.WasCompleted && _shouldContinueAttack)
            {
                _currentAttack = 2;
                Context.Player.Stamina.Spend(Context.PlayerConfig.SecondStrongAttackStaminaCost);
                Context.SecondStrongAttack.StartAttack(AttackEndedSecond);
            }
            else
            {
                AttackEndedSecond(result);
            }
        }

        public override void UpdateState()
        {
            if (!_shouldContinueAttack && Context.Player.HasStamina() && Context.Input.IsStrongAttackPressed)
            {
                Context.AnimatorComponent.SetTrigger("strong-attack");
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