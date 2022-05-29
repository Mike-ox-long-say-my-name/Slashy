using Core.Attacking;
using Core.Characters;
using Core.Characters.Interfaces;
using UnityEngine;

namespace Characters.Enemies.Rogue
{
    public class RogueTripleSlash : RogueBaseState
    {
        public override void EnterState()
        {
            Context.AutoMovement.UnlockRotation();
            Context.AutoMovement.ResetState();
            Context.VelocityMovement.Stop();

            Context.Animator.PlayTripleSlashAnimation();
            Context.TripleSlashExecutor.StartAttack(OnAttackEnded);
            Context.Character.Balance.Frozen = true;
        }

        private void OnAttackEnded(AttackResult obj)
        {
            if (obj.WasInterrupted)
            {
                return;
            }

            var value = Random.value;
            if (value < 0.2)
            {
                SwitchState<RogueWait>();
            }
            else if (value < 0.4f)
            {
                SwitchState<RogueJumpAway>();
            }
            else if (value < 0.8f)
            {
                SwitchState<RogueRetreat>();
            }
            else
            {
                SwitchState<RogueWaitBeforeThrust>();
            }
        }

        public override void ExitState()
        {
            Context.Character.Balance.Frozen = false;
        }
    }
}