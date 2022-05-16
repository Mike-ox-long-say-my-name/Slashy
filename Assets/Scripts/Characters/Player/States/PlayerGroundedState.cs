using System.Collections;
using Core;
using Core.Levels;
using UnityEngine;

namespace Characters.Player.States
{
    public class PlayerGroundedState : PlayerBaseGroundedState
    {
        private bool _locked;

        public override void EnterState()
        {
            _locked = false;
            Context.AttackedThisAirTime = false;
        }

        private IEnumerator MoveToBonfireRoutine(Bonfire bonfire)
        {
            Context.Hurtbox.Disable();

            var bonfirePosition = bonfire.transform.position.WithZeroY();
            var animationPosition = bonfire.GetPlayerAnimationPosition();
            yield return MoveToRoutine(animationPosition);
            Context.VelocityMovement.BaseMovement.Rotate(bonfirePosition.x - Context.Transform.position.x);

            SwitchState<PlayerTouchingBonfireState>();
        }

        private IEnumerator MoveToRoutine(Vector3 position)
        {
            var movement = Context.VelocityMovement;
            var player = movement.BaseMovement.Transform;

            while (Vector3.Distance(player.position.WithZeroY(), position) > 0.1f)
            {
                var direction = (position - player.position).normalized;
                movement.Move(direction);
                yield return null;
            }

            movement.BaseMovement.SetPosition(position);
        }

        public override void UpdateState()
        {
            if (_locked)
            {
                return;
            }

            if (Context.WarpPosition != null)
            {
                _locked = true;
                Context.StartCoroutine(MoveToWarpRoutine(Context.WarpPosition.Value));
                return;
            }

            HandleControl();

            Context.Animator.SetBool("is-walking", Context.Input.MoveInput.sqrMagnitude > 0);

            CheckStateSwitch();
        }

        private IEnumerator MoveToWarpRoutine(Vector3 position)
        {
            Context.Hurtbox.Disable();

            yield return MoveToRoutine(position);

            LevelWarpManager.Instance.PlayerReady();
        }

        public override void OnInteracted()
        {
            var mask = InteractionMask.Any;
            if (_locked)
            {
                mask ^= InteractionMask.Bonfire;
            }

            var result = Context.Interactor.TryInteract(mask);
            if (result.Type != InteractionType.TouchedBonfire)
            {
                return;
            }

            _locked = true;
            Context.StartCoroutine(MoveToBonfireRoutine((Bonfire)result.Sender));
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