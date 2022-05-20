using Core.Attacking;
using Core.Attacking.Interfaces;
using UnityEngine;

namespace Core.Characters.Mono
{
    [RequireComponent(typeof(AudioSource))]
    [RequireComponent(typeof(MixinHittable))]
    public class MixinDamageTakenSound : MonoBehaviour
    {
        private AudioSource _audioSource;
        [SerializeField] private AudioClip damageTakenSound;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        private void OnEnable()
        {
            var hittable = GetComponent<MixinHittable>().HitReceiver;
            hittable.HitReceived += OnHitReceived;
        }

        private void OnDisable()
        {
            var hittable = GetComponent<MixinHittable>().HitReceiver;
            hittable.HitReceived -= OnHitReceived;
        }

        private void OnHitReceived(IHitReceiver arg1, HitInfo arg2)
        {
            if (damageTakenSound)
            {
                _audioSource.PlayOneShot(damageTakenSound);
            }
        }
    }
}