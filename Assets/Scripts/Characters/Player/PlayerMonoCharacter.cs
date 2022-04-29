using System;
using Core.Attacking;
using Core.Characters;
using Core.Characters.Interfaces;
using Core.Player.Interfaces;
using Core.Utilities;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Characters.Player
{
    public class PlayerCharacter : Character, IPlayerCharacter
    {
        public PlayerStats PlayerStats { get; set; }

        private readonly StaminaResource _staminaResource;

        public event Action<IPlayerCharacter, ICharacterResource> OnStaminaChanged;

        IPlayerMovement IPlayerCharacter.PlayerMovement => (IPlayerMovement)VelocityMovement;
        public ICharacterResource Stamina => _staminaResource;

        private readonly TimedTrigger _staminaRecoveryDelayed = new TimedTrigger();

        public PlayerCharacter(IPlayerMovement movement, DamageStats damageStats, CharacterStats characterStats, PlayerStats playerStats)
            : base(movement, damageStats, characterStats)
        {
            PlayerStats = playerStats;
            _staminaResource = new StaminaResource(this, playerStats.MaxStamina);
        }

        public override void Tick(float deltaTime)
        {
            base.Tick(deltaTime);

            RecoverStamina();
            _staminaRecoveryDelayed.Step(deltaTime);
        }

        private void RecoverStamina()
        {
            if (_staminaRecoveryDelayed.IsSet || PlayerStats.FreezeStamina)
            {
                return;
            }

            _staminaResource.Recover(PlayerStats.StaminaRegeneration * Time.deltaTime);
            OnStaminaChanged?.Invoke(this, Stamina);
        }

        public void SpendStamina(float amount)
        {
            if (PlayerStats.FreezeStamina)
            {
                return;
            }

            _staminaResource.Spend(amount);
            if (_staminaResource.IsDepleted)
            {
                var delay = PlayerStats.StaminaRegenerationDelay + PlayerStats.EmptyStaminaAdditionalRegenerationDelay;
                _staminaRecoveryDelayed.SetFor(delay);
            }
            else
            {
                var delay = PlayerStats.StaminaRegenerationDelay;
                _staminaRecoveryDelayed.SetFor(delay);
            }

            OnStaminaChanged?.Invoke(this, Stamina);
        }

        protected override void Die(HitInfo info)
        {
            base.Die(info);
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
