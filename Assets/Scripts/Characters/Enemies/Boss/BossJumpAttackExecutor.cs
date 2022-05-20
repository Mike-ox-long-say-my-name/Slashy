using Core.Attacking;
using Core.Attacking.Interfaces;
using Core.Attacking.Mono;
using UnityEngine;

namespace Characters.Enemies.Boss
{
    public class BossJumpAttackExecutor : MonoAnimationAttackExecutor
    {
        [SerializeField] private BossJumpAttackData attackData;

        private class CustomHandler : DefaultAttackEventHandler
        {
            public Transform Parent { get; set; }
            public BossJumpAttackData AttackData { get; set; }
            private AttackboxGroup _group;

            public override void HandleEnableHitbox(IAnimationAttackExecutorContext context)
            {
                base.HandleEnableHitbox(context);
                CreateWalkers();
            }

            private void CreateWalkers()
            {
                _group = new AttackboxGroup();

                var groundOffset = new Vector3(0, AttackData.GroundOffset, 0);

                var deltaAngle = Mathf.PI * 2 / AttackData.Rows;
                var startPosition = Parent.position + groundOffset;

                var offset = AttackData.BaseOffsetDistance;
                var angle = 0f;
                for (int i = 0; i < AttackData.Rows; i++)
                {
                    var randomOffset = Random.value;
                    var direction = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle));

                    var walker = Instantiate(AttackData.Walker, startPosition + direction * offset * randomOffset, Quaternion.identity);
                    walker.hideFlags = HideFlags.HideAndDontSave;
                    walker.GoInDirection(direction * AttackData.StepDistance, AttackData.StepInterval, position =>
                    {
                        var spike = Instantiate(AttackData.Spike, position, Quaternion.identity);
                        spike.hideFlags = HideFlags.HideAndDontSave;
                        spike.StrikeFromGround(_group);
                    });

                    angle += deltaAngle;
                }
            }
        }

        protected override void ConfigureExecutor(AnimationAttackExecutor executor)
        {
            executor.EventHandler = new CustomHandler
            {
                Parent = transform,
                AttackData = attackData
            };
        }
    }
}