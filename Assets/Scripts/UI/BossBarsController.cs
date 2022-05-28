using Core;
using UnityEngine;

namespace UI
{
    public class BossBarsController : MonoBehaviour
    {
        [SerializeField] private ResourceBar healthBar;

        private BossMarker _boss;

        private void Start()
        {
            _boss = FindObjectOfType<BossMarker>();
            var events = _boss.BossEvents;
            events.FightStarted.AddListener(OnFightStarted);
            events.Died.AddListener(OnDied);
        }

        private void OnDied()
        {
            healthBar.gameObject.SetActive(false);
        }

        private void OnFightStarted()
        {
            var character = _boss.Character.Character;
            healthBar.gameObject.SetActive(true);

            character.Health.ValueChanged += healthBar.OnResourceValueChanged;
            character.Health.ForceRaiseEvent();
        }
    }
}