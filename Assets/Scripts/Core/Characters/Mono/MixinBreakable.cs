using Core.Attacking;
using Core.Attacking.Interfaces;
using Core.Characters.Interfaces;
using Core.Modules;
using UnityEngine;
using UnityEngine.Events;

namespace Core.Characters.Mono
{
    [RequireComponent(typeof(MixinHittable))]
    [RequireComponent(typeof(MixinHealth))]
    public class MixinBreakable : MonoBehaviour
    {
        [SerializeField] private AudioSource breakSoundSource;
        [SerializeField] private GameObject brokenObject;
        [SerializeField] private UnityEvent destroyed;

        public UnityEvent Destroyed => destroyed;

        private IResource _health;

        private void Awake()
        {
            var receiver = GetComponent<MixinHittable>().HitReceiver;
            receiver.HitReceived += ReceiveHit;

            _health = GetComponent<MixinHealth>().Health;
        }

        private void ReceiveHit(IHitReceiver receiver, HitInfo hitInfo)
        {
            _health.Spend(hitInfo.Damage);

            if (_health.IsDepleted())
            {
                brokenObject.SetActive(true);
                breakSoundSource.Play();
                Destroyed?.Invoke();
            }
        }
    }
}
