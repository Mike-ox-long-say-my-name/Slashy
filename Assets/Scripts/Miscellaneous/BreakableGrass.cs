using Core;
using Core.Attacking;
using Core.Attacking.Interfaces;
using Core.Characters.Mono;
using UnityEngine;

namespace Miscellaneous
{
    [RequireComponent(typeof(MixinDestroyable))]
    [RequireComponent(typeof(MixinHittable))]
    public class BreakableGrass : MonoBehaviour
    {
        [SerializeField] private AudioSource breakSoundSource;
        [SerializeField] private GameObject brokenGrass;
        [SerializeField] private GameObject additional;

        private void Awake()
        {
            var hittable = GetComponent<MixinHittable>().HitReceiver;
            hittable.HitReceived += OnHitReceived;
        }

        private void OnHitReceived(IHitReceiver arg1, HitInfo arg2)
        {
            Break();
        }

        public void Break()
        {
            brokenGrass.SetActive(true);
            breakSoundSource.Play();

            if (additional != null)
            {
                additional.SetActive(true);
            }

            var destroyable = GetComponent<MixinDestroyable>();
            destroyable.Destroy();
        }
    }
}
