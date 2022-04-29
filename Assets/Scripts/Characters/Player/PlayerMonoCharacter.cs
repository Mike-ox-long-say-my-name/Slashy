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
        private readonly IPlayerStats _stats;
        private readonly StaminaResource _staminaResource;

        public event Action<IPlayerCharacter, ICharacterResource> OnStaminaChanged;

        IPlayerMovement IPlayerCharacter.Movement => (IPlayerMovement)Movement;
        public ICharacterResource Stamina => _staminaResource;

        private readonly TimedTrigger _staminaRecoveryDelayed = new TimedTrigger();

        public PlayerCharacter(IPlayerMovement movement, IPlayerStats stats)
            : base(movement, movement?.Pushable, stats)
        {
            _stats = stats;
            _staminaResource = new StaminaResource(this, stats.MaxStamina);
        }

        public override void Tick(float deltaTime)
        {
            base.Tick(deltaTime);

            RecoverStamina();
            _staminaRecoveryDelayed.Step(deltaTime);
        }

        private void RecoverStamina()
        {
            if (_staminaRecoveryDelayed.IsSet || _stats.FreezeStamina)
            {
                return;
            }

            _staminaResource.Recover(_stats.StaminaRegeneration * Time.deltaTime);
            OnStaminaChanged?.Invoke(this, Stamina);
        }

        public void SpendStamina(float amount)
        {
            if (_stats.FreezeStamina)
            {
                return;
            }

            _staminaResource.Spend(amount);
            if (_staminaResource.IsDepleted)
            {
                var delay = _stats.StaminaRegenerationDelay + _stats.EmptyStaminaAdditionalRegenerationDelay;
                _staminaRecoveryDelayed.SetFor(delay);
            }
            else
            {
                var delay = _stats.StaminaRegenerationDelay;
                _staminaRecoveryDelayed.SetFor(delay);
            }

            OnStaminaChanged?.Invoke(this, Stamina);
        }

        protected override void Die(HitInfo info)
        {
            base.Die(info);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
