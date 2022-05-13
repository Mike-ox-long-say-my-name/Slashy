using UnityEngine;
using UnityEngine.Events;

namespace Core.Attacking.Mono
{
    public class MixinTriggerEventDispatcher : MonoBehaviour
    {
        [SerializeField] private bool enterAndExitOnce = false;

        [SerializeField] private UnityEvent<Collider> enter = new UnityEvent<Collider>();
        [SerializeField] private UnityEvent<Collider> stay = new UnityEvent<Collider>();
        [SerializeField] private UnityEvent<Collider> exit = new UnityEvent<Collider>();

        public UnityEvent<Collider> Enter => enter;

        public UnityEvent<Collider> Exit => exit;

        public UnityEvent<Collider> Stay => stay;

        private bool _entered = false;
        private bool _exited = false;

        private void OnTriggerEnter(Collider other)
        {
            if (_entered && enterAndExitOnce)
            {
                return;
            }
            Enter?.Invoke(other);
            _entered = true;
        }

        private void OnTriggerStay(Collider other)
        {
            Stay?.Invoke(other);
        }

        private void OnTriggerExit(Collider other)
        {
            if (_exited && enterAndExitOnce)
            {
                return;
            }
            Exit?.Invoke(other);
            _exited = true;
        }
    }
}