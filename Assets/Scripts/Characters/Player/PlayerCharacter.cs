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

        [SerializeField] private PlayerActionConfig actionConfig;
        [SerializeField] private bool freezeStamina = false;
        [SerializeField, Min(0)] private float maxStamina = 0;

        private StaminaResource _staminaResource;
        public ICharacterResource Stamina => _staminaResource;
        public bool HasStamina => Stamina.Value > 0;

        private readonly TimedTriggerFactory _triggerFactory = new TimedTriggerFactory();
        private TimedTrigger _staminaRecoveryDelayed;

        protected override void Awake()
        {
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

            _staminaResource.Recover(actionConfig.StaminaRegeneration * Time.deltaTime);
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
                    actionConfig.StaminaRegenerationDelay + actionConfig.EmptyStaminaAdditionalRegenerationDelay);
            }
            else
            {
                _staminaRecoveryDelayed.SetFor(
                    actionConfig.StaminaRegenerationDelay);
            }
            OnStaminaChanged?.Invoke(Stamina);
        }

        protected override void OnDeath()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
