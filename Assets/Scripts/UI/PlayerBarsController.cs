using System;
using Core;
using Core.Player.Interfaces;
using UnityEngine;

namespace UI
{
    public class PlayerBarsController : MonoBehaviour
    {
        [SerializeField] private ResourceBar healthBar;
        [SerializeField] private ResourceBar staminaBar;
        private IPlayerFactory _playerFactory;

        private void Awake()
        {
            _playerFactory = Container.Get<IPlayerFactory>();
            _playerFactory.WhenPlayerAvailable(SubscribeToPlayerResourceUpdates);
        }

        private void SubscribeToPlayerResourceUpdates(IPlayer player)
        {
            player.Character.Health.ValueChanged += healthBar.OnResourceValueChanged;
            player.Stamina.ValueChanged += staminaBar.OnResourceValueChanged;
        }

        private void OnDestroy()
        {
            _playerFactory.PlayerCreated -= SubscribeToPlayerResourceUpdates;
        }
    }
}