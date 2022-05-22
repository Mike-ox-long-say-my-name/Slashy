using Core.Player.Interfaces;
using UnityEngine;
using UnityEngine.Events;

namespace Core.Attacking.Mono
{
    public class MixinTriggerEventDispatcher : MonoBehaviour
    {
        [SerializeField] private bool reactOnlyOnPlayer = false;

        [SerializeField] private UnityEvent<Collider> enter = new UnityEvent<Collider>();
        [SerializeField] private UnityEvent<Collider> stay = new UnityEvent<Collider>();
        [SerializeField] private UnityEvent<Collider> exit = new UnityEvent<Collider>();

        public UnityEvent<Collider> Enter => enter;

        public UnityEvent<Collider> Exit => exit;

        public UnityEvent<Collider> Stay => stay;

        private bool ShouldDispatch(Collider other)
        {
            return !reactOnlyOnPlayer || other.TryGetComponent<IPlayer>(out _);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (ShouldDispatch(other))
            {
                Enter?.Invoke(other);
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (ShouldDispatch(other))
            {
                Stay?.Invoke(other);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (ShouldDispatch(other))
            {
                Exit?.Invoke(other);
            }
        }
    }
}