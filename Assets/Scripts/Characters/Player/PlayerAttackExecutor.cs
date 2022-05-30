using System;
using Core.Attacking;
using Core.Attacking.Interfaces;
using Core.Attacking.Mono;
using Core.Characters.Interfaces;
using Core.Modules;
using Core.Player.Interfaces;
using UnityEngine;

namespace Characters.Player
{
    public class PlayerAttackExecutor : MonoAnimationAttackExecutor
    {
        private struct AttackContext
        {
            public IAutoPlayerInput Input { get; set; }
            public IBaseMovement BaseMovement { get; set; }
            public float MoveDistance { get; set; }
            public float TimeScaleOnHit { get; set; }
        }

        private class AttackEventHandler : DefaultAttackEventHandler
        {
            private readonly AttackContext _attackContext;

            public AttackEventHandler(AttackContext context)
            {
                _attackContext = context;
            }

            public override void HandleEnableHitbox(IAnimationAttackExecutorContext context)
            {
                _canSlowTime = true;
                context.Attackbox.Hit += SlowTimeOnHit;
                base.HandleEnableHitbox(context);

                var inputX = _attackContext.Input.MoveInput.x;
                _attackContext.BaseMovement.Rotate(inputX);

                var direction = _attackContext.BaseMovement.Transform.right;
                _attackContext.BaseMovement.Move(direction * _attackContext.MoveDistance);
            }

            public override void HandleDisableHitbox(IAnimationAttackExecutorContext context)
            {
                context.Attackbox.Hit -= SlowTimeOnHit;
                ReturnToDefaultTimeScale();
                base.HandleEnableHitbox(context);
            }

            private bool _canSlowTime = false;
            
            private void SlowTimeOnHit(IHurtbox obj)
            {
                if (!_canSlowTime)
                {
                    return;
                }

                _canSlowTime = false;
                Time.timeScale = _attackContext.TimeScaleOnHit;
            }

            public override void HandleAttackEnd(IAnimationAttackExecutorContext context, bool _)
            {
                ReturnToDefaultTimeScale();
                base.HandleAttackEnd(context, _);
            }
        }

        [SerializeField, Min(0)] private float timeScaleOnHit = 1;
        [SerializeField, Min(0)] private float moveDistance = 0.4f;

        protected override void ConfigureExecutor(AnimationAttackExecutor executor)
        {
            var movement = GetComponentInParent<MixinMovementBase>().BaseMovement;
            var playerInput = GetComponentInParent<IAutoPlayerInput>();
            var context = new AttackContext
            {
                Input = playerInput,
                BaseMovement = movement,
                MoveDistance = moveDistance,
                TimeScaleOnHit = timeScaleOnHit
            };
            executor.EventHandler = new AttackEventHandler(context);
        }

        private void OnDisable()
        {
            ReturnToDefaultTimeScale();
        }

        private static void ReturnToDefaultTimeScale()
        {
            Time.timeScale = 1f;
        }
    }
}