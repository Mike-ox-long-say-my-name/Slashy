using System;
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

        private void Start()
        {
            // Прогревочный
            BaseMovement.Move(new Vector3(0.01f, -0.5f, 0));
            BaseMovement.Move(new Vector3(-0.01f, -0.5f, 0));
        }
    }
}