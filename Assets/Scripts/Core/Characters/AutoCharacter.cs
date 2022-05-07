using System;
using Core.Characters.Interfaces;
using Core.Modules;
using UnityEngine;

namespace Core.Characters
{
    [RequireComponent(typeof(MixinMovementBase))]
    public class MixinPushable : MonoBehaviour
    {
        private IPushable _pushable;

        public IPushable Pushable
        {
            get
            {
                if (_pushable != null)
                {
                    return _pushable;
                }

                var movement = GetComponent<MixinMovementBase>().BaseMovement;
                _pushable = new Pushable(movement);
                return _pushable;
            }
        }

        private void Update()
        {
            Pushable.Tick(Time.deltaTime);
        }
    }
}