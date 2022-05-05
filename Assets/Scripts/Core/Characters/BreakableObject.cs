using Core.Attacking;
using Core.Attacking.Interfaces;
using UnityEngine;
using UnityEngine.Events;

namespace Core.Characters
{
    public class BreakableObject : MonoBehaviour, IHitReceiver
    {
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
            if (_health <= 0)
            {
                destroyed?.Invoke();
            }
        }
    }
}
