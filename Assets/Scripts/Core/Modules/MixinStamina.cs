using Core.Characters;
using Core.Characters.Interfaces;
using Core.Characters.Mono;
using Core.Utilities;
using UnityEngine;

namespace Core.Modules
{
    public class MixinStamina : MixinResource
    {
        [SerializeField] private float recoverySpeed = 20;
        [SerializeField] private float staminaRecoveryDelay = 0.5f;
        [SerializeField] private float emptyStaminaAdditionalDelay = 1;

        private readonly Timer _delayTimer = new Timer();

        private bool _canRecover = true;

        private void Awake()
        {
            _delayTimer.Timeout += () => _canRecover = true;
        }

        private void Update()
        {
            _delayTimer.Tick(Time.deltaTime);

            if (_canRecover)
            {
                Stamina.Recover(recoverySpeed * Time.deltaTime);
            }
        }

        private IResource _stamina;
        private float _lastValue;
        private IAggroListener _aggroListener;

        private IResource Stamina
        {
            get
            {
                if (_stamina != null)
                {
                    return _stamina;
                }

                _aggroListener = Container.Get<IAggroListener>();
                _stamina = new CustomStamina(_aggroListener, MaxValue);
                _lastValue = _stamina.Value;
                _stamina.ValueChanged += OnValueChanged;
                return _stamina;
            }
        }

        private class CustomStamina : StaminaResource
        {
            private readonly IAggroListener _aggroListener;

            public CustomStamina(IAggroListener aggroListener, float maxStamina) : base(maxStamina)
            {
                _aggroListener = aggroListener;
            }

            public override void Spend(float amount)
            {
                if (_aggroListener.IsFighting)
                {
                    base.Spend(amount);
                }
            }
        }

        private void OnValueChanged(IResource resource)
        {
            if (_lastValue > resource.Value)
            {
                _canRecover = false;

                var delayTime = staminaRecoveryDelay;
                if (resource.IsDepleted())
                {
                    delayTime += emptyStaminaAdditionalDelay;
                }

                _delayTimer.Start(delayTime);
            }

            _lastValue = resource.Value;
        }

        public override IResource Resource => Stamina;
    }
}