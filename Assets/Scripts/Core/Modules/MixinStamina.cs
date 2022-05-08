using System;
using Core.Characters;
using Core.Characters.Interfaces;
using Core.Characters.Mono;
using Core.Utilities;
using UnityEngine;

namespace Core.Modules
{
    public class MixinStamina : MixinResource
    {
        [SerializeField] private StaminaRecoveryPolicy recoverPolicy;

        private bool _hasRecoverPolicy;

        private void Awake()
        {
            _hasRecoverPolicy = recoverPolicy != null;
        }

        private void Update()
        {
            if (!_hasRecoverPolicy)
            {
                return;
            }

            var deltaTime = Time.deltaTime;
            recoverPolicy.Tick(deltaTime);

            if (recoverPolicy.CanRecover)
            {
                Stamina.Recover(recoverPolicy.RecoverySpeed * deltaTime);
            }
        }

        private IResource _stamina;

        public IResource Stamina
        {
            get
            {
                if (_stamina != null)
                {
                    return _stamina;
                }

                _stamina = new StaminaResource(MaxValue);
                return _stamina;
            }
        }

        public override IResource Resource => Stamina;
    }
}