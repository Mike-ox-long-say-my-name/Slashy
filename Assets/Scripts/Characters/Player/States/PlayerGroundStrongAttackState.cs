using Core.Attacking;
using Core.Characters;
using Core.Characters.Interfaces;

namespace Characters.Player.States
{
    public class PlayerGroundStrongAttackState : PlayerBaseGroundedState
    {
        public override void EnterState()
        {
            Context.VelocityMovement.Stop();

            Context.Animator.PlayFirstGroundHeavyAttack();
            Context.ResourceSpender.SpendFor(PlayerResourceAction.FirstHeavyAttack);

            InflictSelfHit();
            Context.FirstStrongAttack.StartAttack(OnFirstAttackEnded);
        }

        private void InflictSelfHit()
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
            Context.HitReceiver.ReceiveHit(selfHit);
        }

        private void OnFirstAttackEnded(AttackResult result)
        {
            if (ShouldEndCombo(result))
            {
                OnWholeAttackEnded(result);
                return;
            }

            Context.ResourceSpender.SpendFor(PlayerResourceAction.SecondHeavyAttack);
            Context.Animator.PlaySecondGroundHeavyAttack();
            Context.SecondStrongAttack.StartAttack(OnSecondAttackEnded);
        }

        private bool ShouldEndCombo(AttackResult result)
        {
            return !result.WasCompleted ||
                   !Context.ResourceSpender.HasEnoughResourcesFor(PlayerResourceAction.SecondHeavyAttack) ||
                   !Context.Input.IsStrongAttackPressed;
        }

        private void OnSecondAttackEnded(AttackResult result)
        {
            OnWholeAttackEnded(result);
        }

        private void OnWholeAttackEnded(AttackResult result)
        {
            if (result.WasCompleted)
            {
                SwitchState<PlayerGroundedState>();
            }
        }
    }
}