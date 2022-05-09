using Core.Characters.Interfaces;
using Core.Utilities;
using UnityEngine;

namespace Core.Characters
{
    [CreateAssetMenu(menuName = "Policy/Stamina Recovery", fileName = "DelayedStaminaRecoveryPolicy", order = 0)]
    public class DelayedStaminaRecoveryPolicy : StaminaRecoveryPolicy
    {
        [SerializeField] private float recoverySpeed;
        [SerializeField] private float staminaRecoveryDelay;
        [SerializeField] private float emptyStaminaAdditionalDelay;

        private readonly Timer _delayTimer = new Timer();

        private bool _canRecover = true;

        public override float RecoverySpeed => recoverySpeed;
        public override bool CanRecover => _canRecover;

        private void Awake()
        {
            _delayTimer.Timeout += () => _canRecover = true;
        }

        public override void InformSpent(IResource stamina)
        { 
            var delay = staminaRecoveryDelay;
            if (stamina.IsDepleted())
            {
                delay += emptyStaminaAdditionalDelay;
            }

            _canRecover = false;
            _delayTimer.Start(delay);
        }

        public override void Tick(float deltaTime)
        {
            _delayTimer.Tick(deltaTime);
        }
    }
}