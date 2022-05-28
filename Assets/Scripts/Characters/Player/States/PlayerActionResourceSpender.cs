using System;
using Configs.Player;
using Core.Characters;
using Core.Characters.Interfaces;

namespace Characters.Player.States
{
    public class PlayerActionResourceSpender : IPlayerActionResourceSpender
    {
        private readonly PlayerConfig _playerConfig;
        private readonly IResource _healthResource;
        private readonly IResource _staminaResource;

        public PlayerActionResourceSpender(PlayerConfig playerConfig, IResource healthResource,
            IResource staminaResource)
        {
            _playerConfig = playerConfig;
            _healthResource = healthResource;
            _staminaResource = staminaResource;
        }

        public bool HasEnoughResourcesFor(PlayerResourceAction action)
        {
            var hasEnough = _staminaResource.HasAny();
            if (action == PlayerResourceAction.FirstHeavyAttack)
            {
                hasEnough &= _healthResource.Value > _playerConfig.FirstStrongAttackHealthCost;
            }

            return hasEnough;
        }

        public void SpendFor(PlayerResourceAction action)
        {
            switch (action)
            {
                case PlayerResourceAction.Dash:
                    _staminaResource.Spend(_playerConfig.DashStaminaCost);
                    break;
                case PlayerResourceAction.Jump:
                    _staminaResource.Spend(_playerConfig.JumpStaminaCost);
                    break;
                case PlayerResourceAction.FirstLightAttack:
                    _staminaResource.Spend(_playerConfig.LightAttackFirstStaminaCost);
                    break;
                case PlayerResourceAction.SecondLightAttack:
                    _staminaResource.Spend(_playerConfig.LightAttackSecondStaminaCost);
                    break;
                case PlayerResourceAction.FirstHeavyAttack:
                    _staminaResource.Spend(_playerConfig.FirstStrongAttackStaminaCost);
                    break;
                case PlayerResourceAction.SecondHeavyAttack:
                    _staminaResource.Spend(_playerConfig.SecondStrongAttackStaminaCost);
                    break;
                case PlayerResourceAction.HealTick:
                    _staminaResource.Spend(_playerConfig.HealStaminaConsumptionPerTick);
                    break;
                case PlayerResourceAction.AirAttack:
                    _staminaResource.Spend(_playerConfig.LightAirAttackStaminaCost);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(action), action, null);
            }
        }
    }
}