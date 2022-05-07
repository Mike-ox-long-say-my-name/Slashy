using Core.Characters;
using Core.Characters.Interfaces;
using UnityEngine;

namespace Core.Modules
{
    [RequireComponent(typeof(CharacterController))]
    public class MixinMovementBase : MonoBehaviour
    {
        private IBaseMovement _movement;

        public IBaseMovement BaseMovement
        {
            get
            {
                if (_movement != null)
                {
                    return _movement;
                }

                var controller = GetComponent<CharacterController>();
                _movement = new BaseMovement(controller);
                return _movement;
            }
        }
    }
}