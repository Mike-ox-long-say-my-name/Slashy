using Core.Attacking.Interfaces;
using UnityEngine;

namespace Core.Characters
{
    public class MixinHittable : MonoBehaviour
    {
        public IHitReceiver HitReceiver { get; } = new HitReceiver();
    }
}