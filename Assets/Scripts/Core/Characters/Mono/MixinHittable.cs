using Core.Attacking.Interfaces;
using UnityEngine;

namespace Core.Characters.Mono
{
    public class MixinHittable : MonoBehaviour
    {
        public IHitReceiver HitReceiver { get; } = new HitReceiver();
    }
}