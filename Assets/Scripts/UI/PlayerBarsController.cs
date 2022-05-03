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
            player.Health.OnValueChanged += healthBar.OnResourceValueChanged;
            player.Stamina.OnValueChanged += staminaBar.OnResourceValueChanged;
        }

        private void OnDisable()
        {
            var player = PlayerManager.Instance.PlayerInfo?.Player;
            if (player == null)
            {
                return;
            }
            player.Health.OnValueChanged -= healthBar.OnResourceValueChanged;
            player.Stamina.OnValueChanged -= staminaBar.OnResourceValueChanged;
        }
    }
}
