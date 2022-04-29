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
            if (player == null)
            {
                return;
            }
            player.OnHealthChanged += healthBar.OnResourceValueChanged;
            player.OnStaminaChanged += staminaBar.OnResourceValueChanged;
        }

        private void OnDisable()
        {
            var player = PlayerManager.Instance.PlayerInfo?.Player;
            if (player == null)
            {
                return;
            }
            player.OnHealthChanged -= healthBar.OnResourceValueChanged;
            player.OnStaminaChanged -= staminaBar.OnResourceValueChanged;
        }
    }
}
