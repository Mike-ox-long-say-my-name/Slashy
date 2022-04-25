using Core;
using Core.Attacking;
using Core.Attacking.Interfaces;
using Core.Attacking.Mono;
using Core.Characters.Interfaces;
using Core.Player.Interfaces;
using UnityEngine;

namespace Characters.Player
{
    public class PlayerLightAttackExecutor : MonoAnimationAttackExecutor
    {
        private struct AttackContext
        {
            public IAutoPlayerInput Input { get; set; }
            public IPlayerMovement Movement { get; set; }
            public float MoveDistance { get; set; }
        }

        private class PlayerLightAttack : AnimationAttackExecutor
        {
            private readonly AttackContext _context;

            public PlayerLightAttack(AttackContext context, ICoroutineHost host, IAttackbox attackbox) : base(host, attackbox)
            {
                _context = context;
            }

            public override void OnEnableHitbox()
            {
                base.OnEnableHitbox();
                var inputX = _context.Input.MoveInput.x;
                _context.Movement.Rotate(inputX);

                var direction = _context.Movement.Transform.right;
                _context.Movement.MoveRaw(direction * _context.MoveDistance);
            }
        }

        [SerializeField, Min(0)] private float moveDistance = 0.4f;

        protected override IAttackExecutor CreateExecutor(ICoroutineHost host, IAttackbox attackbox)
        {
            var playerMovement = GetComponentInParent<IMonoPlayerCharacter>()?.Resolve()?.Movement;
            var playerInput = GetComponentInParent<IAutoPlayerInput>();
            var context = new AttackContext
            {
                Input = playerInput,
                Movement = playerMovement,
                MoveDistance = moveDistance
            };
            return new PlayerLightAttack(context, host, attackbox);
        }
    }
}