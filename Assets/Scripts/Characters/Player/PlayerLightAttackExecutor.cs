using Core.Attacking;
using Core.Attacking.Interfaces;
using Core.Attacking.Mono;
using Core.Characters.Interfaces;
using Core.Modules;
using Core.Player.Interfaces;
using UnityEngine;

namespace Characters.Player
{
    public class PlayerLightAttackExecutor : MonoAnimationAttackExecutor
    {
        private struct AttackContext
        {
            public IAutoPlayerInput Input { get; set; }
            public IBaseMovement BaseMovement { get; set; }
            public float MoveDistance { get; set; }
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
                base.HandleEnableHitbox(context);

                var inputX = _attackContext.Input.MoveInput.x;
                _attackContext.BaseMovement.Rotate(inputX);

                var direction = _attackContext.BaseMovement.Transform.right;
                _attackContext.BaseMovement.Move(direction * _attackContext.MoveDistance);
            }
        }

        [SerializeField, Min(0)] private float moveDistance = 0.4f;

        protected override void ConfigureExecutor(AnimationAttackExecutor executor)
        {
            var movement = GetComponentInParent<MixinMovementBase>().BaseMovement;
            var playerInput = GetComponentInParent<IAutoPlayerInput>();
            var context = new AttackContext
            {
                Input = playerInput,
                BaseMovement = movement,
                MoveDistance = moveDistance
            };
            executor.EventHandler = new AttackEventHandler(context);
        }
    }
}