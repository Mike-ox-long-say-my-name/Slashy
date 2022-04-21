using System.Linq;
using Configs;
using Core.Characters;
using Core.Utilities;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Characters.Player
{
    public class PlayerCharacter : Character
    {
        [field: SerializeField]
        public UnityEvent<ICharacterResource> OnStaminaChanged { get; private set; }
            = new UnityEvent<ICharacterResource>();

        [SerializeField] private PlayerConfig config;
        [SerializeField] private bool freezeStamina = false;
        [SerializeField, Min(0)] private float maxStamina = 0;

        private StaminaResource _staminaResource;
        public ICharacterResource Stamina => _staminaResource;
        public bool HasStamina => Stamina.Value > 0;

        private readonly TimedTriggerFactory _triggerFactory = new TimedTriggerFactory();
        private TimedTrigger _staminaRecoveryDelayed;

        protected override void Awake()
        {
            if (config == null)
            {
                Debug.LogWarning("Config is not assigned", this);
                enabled = false;
            }

            base.Awake();
            _staminaResource = new StaminaResource(this, maxStamina);
            _staminaRecoveryDelayed = _triggerFactory.Create();
        }

        private void Update()
        {
            RecoverStamina();
            _triggerFactory.StepAll(Time.deltaTime);
        }

        private void RecoverStamina()
        {
            if (_staminaRecoveryDelayed.IsSet || freezeStamina)
            {
                return;
            }

            _staminaResource.Recover(config.StaminaRegeneration * Time.deltaTime);
            OnStaminaChanged?.Invoke(Stamina);
        }

        public void SpendStamina(float amount)
        {
            if (freezeStamina)
            {
                return;
            }

            _staminaResource.Spend(amount);
            if (_staminaResource.IsDepleted)
            {
                _staminaRecoveryDelayed.SetFor(
                    config.StaminaRegenerationDelay + config.EmptyStaminaAdditionalRegenerationDelay);
            }
            else
            {
                _staminaRecoveryDelayed.SetFor(
                    config.StaminaRegenerationDelay);
            }
            OnStaminaChanged?.Invoke(Stamina);
        }

        protected override void OnDeath()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            var save = SavingMachine.Instance.GetSave(false);
            
            _staminaResource.Value = save.Stamina;
        }
    }
}
