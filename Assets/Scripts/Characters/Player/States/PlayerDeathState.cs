using Core.Characters.Interfaces;
using Core.Player;
using System.Collections;
using Core;
using UnityEngine;

namespace Characters.Player.States
{
    public class PlayerDeathState : PlayerBaseState
    {
        public override void EnterState()
        {
            Context.Hurtbox.Disable();
            Context.VelocityMovement.Stop();
            Context.AnimatorComponent.SetTrigger("death");
            Context.StartCoroutine(PlayDeathSequence(1.4f, 0.02f));
        }

        private static IEnumerator PlayDeathSequence(float slowPerSecond, float tickInterval)
        {
            PlayerManager.Instance.PlayedDeadSequenceStarted?.Invoke();

            const float targetTimeScale = 0.2f;
            while (Time.timeScale > targetTimeScale)
            {
                Time.timeScale = Mathf.Max(targetTimeScale, Time.timeScale - slowPerSecond * Time.unscaledDeltaTime);
                yield return new WaitForSecondsRealtime(tickInterval);
            }

            Time.timeScale = 1f;
            
            BorderManager.Instance.ResetAggroCounter();
            PlayerManager.Instance.PlayedDeadSequenceEnded?.Invoke();
        }
    }
}