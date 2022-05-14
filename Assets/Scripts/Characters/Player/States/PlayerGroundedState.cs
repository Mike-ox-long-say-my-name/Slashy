using System.Collections;
using Core;
using Core.Levels;
using UnityEngine;

namespace Characters.Player.States
{
    public class PlayerGroundedState : PlayerBaseGroundedState
    {
        private bool _movingToBonfire;

        public override void EnterState()
        {
            _movingToBonfire = false;
            Context.AttackedThisAirTime = false;
        }

        private IEnumerator MoveToBonfireRoutine(Bonfire bonfire)
        {
            Context.Hurtbox.Disable();

            var bonfirePosition = bonfire.transform.position.WithZeroY();
            var animationPosition = bonfire.GetPlayerAnimationPosition();
            var movement = Context.VelocityMovement;
            var player = movement.BaseMovement.Transform;

            while (Vector3.Distance(player.position.WithZeroY(), animationPosition) > 0.1f)
            {
                var direction = (animationPosition - player.position).normalized;
                movement.Move(direction);
                yield return null;
            }

            movement.BaseMovement.SetPosition(animationPosition);
            movement.BaseMovement.Rotate(bonfirePosition.x - animationPosition.x);

            SwitchState<PlayerTouchingBonfireState>();
        }

        public override void UpdateState()
        {
            if (_movingToBonfire)
            {
                return;
            }

            HandleControl();

            Context.Animator.SetBool("is-walking", Context.Input.MoveInput.sqrMagnitude > 0);

            CheckStateSwitch();
        }

        public override void OnInteracted()
        {
            var mask = InteractionMask.Any;
            if (_movingToBonfire)
            {
                mask ^= InteractionMask.Bonfire;
            }

            var result = Context.Interactor.TryInteract(mask);
            if (result.Type == InteractionType.TouchedBonfire)
            {
                _movingToBonfire = true;
                Context.StartCoroutine(MoveToBonfireRoutine((Bonfire) result.Sender));
            }
        }

        protected virtual void CheckStateSwitch()
        {
            if (!Context.VelocityMovement.BaseMovement.IsGrounded)
            {
                SwitchState<PlayerFallState>();
            }
            else if (Context.ShouldDash)
            {
                Context.Input.ResetBufferedInput();
                SwitchState<PlayerDashState>();
            }
            else if (Context.ShouldJump)
            {
                Context.Input.ResetBufferedInput();
                SwitchState<PlayerJumpState>();
            }
            else if (Context.ShouldHeal)
            {
                Context.Input.ResetBufferedInput();
                SwitchState<PlayerHealState>();
            }
            else if (Context.ShouldLightAttack)
            {
                Context.Input.ResetBufferedInput();
                SwitchState<PlayerGroundLightAttackState>();
            }
            else if (Context.ShouldStrongAttack)
            {
                Context.Input.ResetBufferedInput();
                SwitchState<PlayerGroundStrongAttackState>();
            }
        }
    }
}