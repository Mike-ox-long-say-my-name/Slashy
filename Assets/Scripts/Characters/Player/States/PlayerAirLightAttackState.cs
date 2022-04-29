using Core.Attacking;
using UnityEngine;

namespace Characters.Player.States
{
    public class PlayerAirLightAttackState : PlayerAirboneState
    {
        public override void EnterState()
        {
            base.EnterState();
            Context.AttackedAtThisAirTime = true;

            Context.AnimatorComponent.SetTrigger("attack");

            var inputX = Context.Input.MoveInput.x;
            var push = Vector3.up;
            if (!Mathf.Approximately(inputX, 0))
            {
                var moveDirection = Mathf.Sign(inputX);
                push += Vector3.right * moveDirection;
            }

            Context.Character.PlayerMovement.Pushable.Push(push, 5, 0.1f);
            Context.LightAirboneAttack.StartAttack(OnAttackEnded);
        }

        private void OnAttackEnded(bool _)
        {
            if (Context.VelocityMovement.Movement.IsGrounded)
            {
                SwitchState<PlayerGroundedState>();
            }
            else
            {
                SwitchState<PlayerFallState>();
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