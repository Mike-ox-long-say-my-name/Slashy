using Core.Characters.Interfaces;
using Core.Modules;
using UnityEngine;

namespace Core.Characters.Mono
{
    [RequireComponent(typeof(MixinMovementBase))]
    [RequireComponent(typeof(MixinVelocityMovement))]
    public class MixinAutoMovement : MonoBehaviour
    {
        private IAutoMovement _autoMovement;

        public IAutoMovement AutoMovement
        {
            get
            {
                if (_autoMovement != null)
                {
                    return _autoMovement;
                }

                var velocityMovement = GetComponent<MixinVelocityMovement>().VelocityMovement;
                _autoMovement = new AutoMovement(velocityMovement);
                return _autoMovement;
            }
        }

        private void Update()
        {
            AutoMovement.Tick(Time.deltaTime);
        }
    }
}