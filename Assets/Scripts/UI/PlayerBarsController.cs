using Core;
using Core.Characters.Interfaces;
using Core.Player.Interfaces;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class PlayerBarsController : MonoBehaviour
    {
        [SerializeField] private Color unpureColor;
        [SerializeField] private Image affectedByPurity;
        [SerializeField] private ResourceBar healthBar;
        [SerializeField] private ResourceBar staminaBar;
        private IPlayerFactory _playerFactory;
        private Color _initialColor;

        private void Awake()
        {
            _initialColor = affectedByPurity.color;
            _playerFactory = Container.Get<IPlayerFactory>();
            _playerFactory.WhenPlayerAvailable(SubscribeToPlayerResourceUpdates);
        }

        private void SubscribeToPlayerResourceUpdates(IPlayer player)
        {
            player.Character.Health.ValueChanged += healthBar.OnResourceValueChanged;
            player.Stamina.ValueChanged += staminaBar.OnResourceValueChanged;
            player.Purity.ValueChanged += OnPurityChanged;
        }

        private void OnPurityChanged(IResource purity)
        {
            var fraction = purity.Value / purity.MaxValue;
            var color = Color.Lerp(unpureColor, _initialColor, fraction);
            affectedByPurity.color = color;
        }

        private void OnDisable()
        {
            _playerFactory.PlayerCreated -= SubscribeToPlayerResourceUpdates;
        }
    }
}