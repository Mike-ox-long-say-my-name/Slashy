using Core.Attacking;
using Core.Attacking.Interfaces;
using UnityEngine;
using UnityEngine.Events;

namespace Core.Characters
{
    public class BreakableObject : MonoBehaviour, IHitReceiver, IHitReceiveDispatcher
    {
        [SerializeField] private UnityEvent<IHitReceiver, HitInfo> hitReceived;
        [SerializeField] private UnityEvent destroyed;
        [SerializeField] private float maxHealth;

        private float _health = 30;

        private void Awake()
        {
            _health = maxHealth;
        }

        public void ReceiveHit(HitInfo hitInfo)
        {
            _health -= hitInfo.Damage;
            HitReceived?.Invoke(this, hitInfo);
            if (_health <= 0)
            {
                destroyed?.Invoke();
            }
        }

        public UnityEvent<IHitReceiver, HitInfo> HitReceived => hitReceived;
    }
}
