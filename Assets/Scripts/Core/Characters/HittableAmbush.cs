using Core.Attacking;
using Core.Attacking.Interfaces;
using Core.Characters.Mono;
using UnityEngine;

namespace Core
{
    public class HittableAmbush : MixinHittable
    {
        [SerializeField] private Ambush ambush;
        
        private void Reset()
        {
            ambush = GetComponent<Ambush>();
        }

        private void Awake()
        {
            HitReceiver.HitReceived += OnHitReceived;
        }

        private void OnHitReceived(IHitReceiver arg1, HitInfo arg2)
        {
            ambush.ActivateAmbush();
        }
    }
}