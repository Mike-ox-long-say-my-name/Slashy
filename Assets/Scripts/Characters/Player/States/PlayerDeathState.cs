using Core;
using Core.Characters.Interfaces;
using Core.Player;
using System.Collections;
using Core.Attacking;
using UnityEngine;

namespace Characters.Player.States
{
    public class PlayerDeathState : PlayerBaseState
    {
        public override void OnDeath(HitInfo info)
        {
        }

        public override void OnStaggered(HitInfo info)
        {
        }

        public override void EnterState()
        {
            Context.Hurtbox.Disable();
            Context.VelocityMovement.Stop();
            Context.Animator.SetTrigger("death");
            Context.StartCoroutine(PlayDeathSequence());
        }

        private static IEnumerator PlayDeathSequence()
        {
            PlayerManager.Instance.PlayedDeadSequenceStarted?.Invoke();
            
            yield return SlowTimeTemporary(1.5f, 0.4f);
            var time = BlackScreenManager.Instance.DefaultTime;
            BlackScreenManager.Instance.Blackout(time);
            yield return new WaitForSeconds(time);
            
            BorderManager.Instance.ResetAggroCounter();
            PlayerManager.Instance.PlayedDeadSequenceEnded?.Invoke();
        }

        private static IEnumerator SlowTimeTemporary(float time, float target)
        {
            var halfTime = time / 2;
            var speed = (Time.timeScale - target) / halfTime;
            var passedTime = 0f;
            int sign = -1;
            while (passedTime < time)
            {
                var deltaTime = Time.unscaledDeltaTime;
                passedTime += deltaTime;

                if (sign == -1 && passedTime >= halfTime)
                {
                    Time.timeScale = target;
                    sign = 1;
                }

                Time.timeScale += sign * deltaTime * speed;
                yield return null;
            }

            Time.timeScale = 1;
        }
    }
}