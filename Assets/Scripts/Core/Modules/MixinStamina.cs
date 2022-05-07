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

    public class ResourceRecovery : IResourceRecovery
    {
        public float RecoverSpeed { get; set; }

        private readonly IResource _resource;
        private readonly Timer _delayTimer = new Timer();

        private bool _canRecover = true;

        public ResourceRecovery(IResource resource, float recoverSpeed)
        {
            _resource = resource;
            RecoverSpeed = recoverSpeed;
            _delayTimer.Timeout += () => _canRecover = true;
        }

        public void Tick(float deltaTime)
        {
            if (!_canRecover)
            {
                return;
            }

            var amount = deltaTime * RecoverSpeed;
            _resource.Recover(amount);
        }

        public void Delay(float delay)
        {
            if (_delayTimer.TimeRemained > delay)
            {
                return;
            }

            _canRecover = false;
            _delayTimer.Start(delay);
        }
    }

    public interface IUpdateable
    {
        void Tick(float deltaTime);
    }

    public interface IResourceRecovery : IUpdateable
    {
        float RecoverSpeed { get; set; }

        void Delay(float delay);
    }

    [RequireComponent(typeof(MixinStamina))]
    public class MixinStaminaRecovery : MonoBehaviour
    {
        [SerializeField] private float recoverSpeed;

        private IResourceRecovery _recovery;

        public IResourceRecovery Recovery
        {
            get
            {
                if (_recovery != null)
                {
                    return _recovery;
                }

                var stamina = GetComponent<MixinStamina>().Stamina;
                _recovery = new ResourceRecovery(stamina, recoverSpeed);
                return _recovery;
            }
        }

        private void Update()
        {
            _recovery.Tick(Time.deltaTime);
        }
    }
}