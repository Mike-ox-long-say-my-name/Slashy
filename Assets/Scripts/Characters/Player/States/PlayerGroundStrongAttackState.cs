using Core.Attacking;
using Core.Characters;
using Core.Characters.Interfaces;

namespace Characters.Player.States
{
    public class PlayerGroundStrongAttackState : PlayerBaseGroundedState
    {
        private bool _shouldContinueAttack;

        public override void EnterState()
        {
            Context.VelocityMovement.Stop();

            Context.Animator.SetTrigger("strong-attack");
            Context.Player.Stamina.Spend(Context.PlayerConfig.FirstStrongAttackStaminaCost);
            
            _shouldContinueAttack = false;

            SelfHit();
            Context.FirstStrongAttack.StartAttack(AttackEndedFirst);
        }

        private void SelfHit()
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
            Context.HitReceiver.ReceiveHit(selfHit);
        }

        private void AttackEndedFirst(AttackResult result)
        {
            if (result.WasCompleted && _shouldContinueAttack)
            {
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
                Context.Animator.SetTrigger("strong-attack");
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
                Context.Animator.ResetTrigger("strong-attack");
            }
        }
    }
}