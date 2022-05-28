using Core.Attacking;
using Core.Utilities;
using UnityEngine;

namespace Characters.Enemies.ExplodingHollow
{
    public class ExplodingHollowRunning : ExplodingHollowBaseState
    {
        private Timer _dotTimer;

        public override void EnterState()
        {
            _dotTimer = Timer.Start(Context.DotTickInterval, SelfHit, true);

            Context.Animator.PlayChargeEndAnimation();
            if (Context.ChargeBurnParticles != null)
            {
                Context.ChargeBurnParticles.Play();
            }
        }

        private void SelfHit()
        {
            Context.Character.HitReceiver.ReceiveHit(new HitInfo
            {
                Multipliers = DamageMultipliers.One,
                DamageStats = Context.DamageStats,
                Source = HitSource.AsCharacter(Context.Character)
            });
        }

        public override void UpdateState()
        {
            var speedMultiplier = 3.5f;
            var player = Context.PlayerPosition;
            var self = Context.transform.position;
            var direction = player - self;
            direction.y = 0;

            if (direction.magnitude > 1.5)
            {
                direction.Normalize();
                Context.VelocityMovement.Move(speedMultiplier * new Vector3(direction.x, 0, direction.z));

                var health = Context.Character.Health;
                if (health.Value / health.MaxValue < 0.2f)
                {
                    SwitchState<ExplodingHollowExplosion>();
                }
            }
            else
            {
                direction.Normalize();
                Context.Pushable.Push(direction, 5f, 0.3f);
                SwitchState<ExplodingHollowExplosion>();
            }

            _dotTimer.Tick(Time.deltaTime);
        }

        public override void OnHitReceived(HitInfo info)
        {
        }

        public override void ExitState()
        {
            _dotTimer.Stop();
        }
    }
}