﻿using UnityEngine;
using UnityEngine.Events;

namespace Core.Attacking.Mono
{
    public class MixinTriggerEventDispatcher : MonoBehaviour
    {
        [SerializeField] private UnityEvent<Collider> enter = new UnityEvent<Collider>();
        [SerializeField] private UnityEvent<Collider> stay = new UnityEvent<Collider>();
        [SerializeField] private UnityEvent<Collider> exit = new UnityEvent<Collider>();

        public UnityEvent<Collider> Enter => enter;

        public UnityEvent<Collider> Exit => exit;

        public UnityEvent<Collider> Stay => stay;

        private void OnTriggerEnter(Collider other)
        {
            Enter?.Invoke(other);
        }

        private void OnTriggerStay(Collider other)
        {
            Stay?.Invoke(other);
        }

        private void OnTriggerExit(Collider other)
        {
            Exit?.Invoke(other);
        }
    }
}