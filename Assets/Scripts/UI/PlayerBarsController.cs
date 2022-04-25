using Core.Player;
using UnityEngine;

namespace UI
{
    public class PlayerBarsController : MonoBehaviour
    {
        [SerializeField] private ResourceBar healthBar;
        [SerializeField] private ResourceBar staminaBar;

        private void OnEnable()
        {
            var player = PlayerManager.Instance.PlayerInfo?.Player;
            player?.OnHealthChanged.AddListener(healthBar.OnResourceValueChanged);
            player?.OnStaminaChanged.AddListener(staminaBar.OnResourceValueChanged);
        }

        private void OnDisable()
        {
            var player = PlayerManager.Instance.PlayerInfo?.Player;
            player?.OnHealthChanged.RemoveListener(healthBar.OnResourceValueChanged);
            player?.OnStaminaChanged.RemoveListener(staminaBar.OnResourceValueChanged);
        }
    }
}
