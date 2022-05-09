using UnityEngine;

namespace Core.Modules
{
    [RequireComponent(typeof(MixinVelocityMovement))]
    public class MixinJumpHandler : MonoBehaviour
    {
        [SerializeField] private float jumpPower;

        private IJumpHandler _jumpHandler;

        public IJumpHandler JumpHandler
        {
            get
            {
                if (_jumpHandler != null)
                {
                    return _jumpHandler;
                }
                
                var baseMovement = GetComponent<MixinMovementBase>().BaseMovement;
                var velocityMovement = GetComponent<MixinVelocityMovement>().VelocityMovement;
                _jumpHandler = new JumpHandler(baseMovement, velocityMovement, jumpPower);
                return _jumpHandler;
            }
        }
    }
}