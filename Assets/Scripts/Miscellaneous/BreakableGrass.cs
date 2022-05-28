using System;
using Core;
using Core.Attacking;
using Core.Attacking.Interfaces;
using Core.Characters.Mono;
using UnityEngine;

namespace Miscellaneous
{
    [RequireComponent(typeof(DestroyHelper))]
    [RequireComponent(typeof(MixinHittable))]
    public class BreakableGrass : MonoBehaviour
    {
        [SerializeField] private AudioSource breakSoundSource;
        [SerializeField] private GameObject brokenGrass;

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

            var destroyable = GetComponent<DestroyHelper>();
            destroyable.Destroy();
        }
    }
}
