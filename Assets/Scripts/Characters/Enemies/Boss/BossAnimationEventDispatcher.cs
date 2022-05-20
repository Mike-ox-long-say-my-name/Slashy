using Core.Attacking.Mono;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;

namespace Characters.Enemies.Boss
{
    public class BossAnimationEventDispatcher : MixinAttackAnimationEventDispatcher
    {
        [SerializeField] private UnityEvent aimSpike;

        public UnityEvent AimSpike => aimSpike;

        [UsedImplicitly]
        public void OnAnimationAimSpike()
        {
            AimSpike?.Invoke();
        }
    }
}