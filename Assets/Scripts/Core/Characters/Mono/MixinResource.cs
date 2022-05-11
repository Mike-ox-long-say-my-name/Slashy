using System;
using Core.Characters.Interfaces;
using UnityEngine;

namespace Core.Characters.Mono
{
    public abstract class MixinResource : MonoBehaviour
    {
        [SerializeField] private float maxValue;

        public float MaxValue => maxValue;

        public abstract IResource Resource { get; }

        private void Start()
        {
            Resource.ForceRaiseEvent();
        }
    }
}