using System.Collections;
using Core;
using Core.Characters.Interfaces;
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
            Context.StartCoroutine(QuitAfterSlowDown(1.4f, 0.02f));
        }

        private static IEnumerator QuitAfterSlowDown(float slowPerSecond, float tickInterval)
        {
            while (Time.timeScale > 0)
            {
                Time.timeScale = Mathf.Max(0, Time.timeScale - slowPerSecond * Time.unscaledDeltaTime);
                yield return new WaitForSecondsRealtime(tickInterval);
            }

            Time.timeScale = 1f;
            GameLoader.Instance.LoadMenu();
        }
    }
}