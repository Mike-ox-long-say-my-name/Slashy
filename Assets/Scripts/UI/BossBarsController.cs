using Core.Characters;
using Core.Characters.Mono;
using UnityEngine;

namespace UI
{
    public class BossBarsController : MonoBehaviour
    {
        [SerializeField] private ResourceBar healthBar;
        [SerializeField] private MixinBossEventDispatcher bossEvents;
        [SerializeField] private MixinCharacter bossCharacter;

        private void OnEnable()
        {
            bossEvents.FightStarted.AddListener(OnFightStarted);
            bossEvents.Died.AddListener(OnDied);
        }

        private void OnDisable()
        {
            bossEvents.FightStarted.RemoveListener(OnFightStarted);
            bossEvents.Died.RemoveListener(OnDied);
        }

        private void OnDied()
        {
            healthBar.gameObject.SetActive(false);
        }

        private void OnFightStarted()
        {
            healthBar.gameObject.SetActive(true);
            bossCharacter.Character.Health.ValueChanged += healthBar.OnResourceValueChanged;
            bossCharacter.Character.Health.ForceRaiseEvent();
        }
    }
}