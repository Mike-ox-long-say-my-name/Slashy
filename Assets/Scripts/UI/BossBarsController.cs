using Core;
using UnityEngine;

namespace UI
{
    public class BossBarsController : MonoBehaviour
    {
        [SerializeField] private ResourceBar healthBar;

        private BossMarker _bossMarker;

        private void Awake()
        {
            _bossMarker = FindObjectOfType<BossMarker>();
            _bossMarker.Created += boss =>
            {
                var events = _bossMarker.BossEvents;
                events.FightStarted.AddListener(OnFightStarted);
                events.Died.AddListener(OnDied);
            };
        }

        private void OnDied()
        {
            healthBar.gameObject.SetActive(false);
        }

        private void OnFightStarted()
        {
            var character = _bossMarker.Character.Character;
            healthBar.gameObject.SetActive(true);

            character.Health.ValueChanged += healthBar.OnResourceValueChanged;
            character.Health.ForceRaiseEvent();
        }
    }
}