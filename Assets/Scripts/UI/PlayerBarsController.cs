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
            player.Character.Health.ValueChanged += healthBar.OnResourceValueChanged;
            player.Stamina.ValueChanged += staminaBar.OnResourceValueChanged;
        }

        private void OnDisable()
        {
            var manager = PlayerManager.TryGetInstance();
            if (manager == null)
            {
                return;
            }
            var player = manager.PlayerInfo?.Player;
            if (player == null)
            {
                return;
            }

            player.Character.Health.ValueChanged -= healthBar.OnResourceValueChanged;
            player.Stamina.ValueChanged -= staminaBar.OnResourceValueChanged;
        }
    }
}
