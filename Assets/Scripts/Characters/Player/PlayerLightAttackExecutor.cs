using Core;
using Core.Attacking;
using Core.Attacking.Interfaces;
using Core.Attacking.Mono;
using Core.Characters.Interfaces;
using Core.Player.Interfaces;
using UnityEngine;

namespace Characters.Player
{
    public class PlayerLightAttackExecutor : MonoAnimationAttackHandler
    {
        private struct AttackContext
        {
            public IAutoPlayerInput Input { get; set; }
            public IBaseMovement BaseMovement { get; set; }
            public float MoveDistance { get; set; }
        }

        private class PlayerLightAttack : AnimationAttackExecutor
        {
            private readonly AttackContext _context;

            public PlayerLightAttack(AttackContext context, ICoroutineHost host, IAttackbox attackbox)
                : base(host, attackbox)
            {
                _context = context;
            }

            public override void OnEnableHitbox()
            {
                base.OnEnableHitbox();
                var inputX = _context.Input.MoveInput.x;
                _context.BaseMovement.Rotate(inputX);

                var direction = _context.BaseMovement.Transform.right;
                _context.BaseMovement.Move(direction * _context.MoveDistance);
            }
        }

        [SerializeField, Min(0)] private float moveDistance = 0.4f;

        protected override AnimationAttackExecutor CreateAnimationAttackExecutor(ICoroutineHost host, IAttackbox attackbox)
        {
            var playerMovement = GetComponentInParent<PlayerMixinCharacter>().Player.PlayerMovement;
            var playerInput = GetComponentInParent<IAutoPlayerInput>();
            var context = new AttackContext
            {
                Input = playerInput,
                BaseMovement = playerMovement.BaseMovement,
                MoveDistance = moveDistance
            };
            return new PlayerLightAttack(context, host, attackbox);
        }
    }
}