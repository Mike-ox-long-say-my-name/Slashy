using Core;
using Core.Attacking.Mono;
using Core.Characters.Mono;
using UnityEngine;

namespace Characters.Enemies.Boss
{
    [RequireComponent(typeof(MixinDamageSource))]
    [RequireComponent(typeof(MixinDestroyable))]
    public class BossSpike : MonoBehaviour
    {
        [SerializeField] private MonoAbstractAttackExecutor strikeAttackExecutor;

        private MixinDestroyable _destroyable;

        private void Awake()
        {
            _destroyable = GetComponent<MixinDestroyable>();
        }

        public void StrikeFromGround()
        {
            var executor = strikeAttackExecutor.GetExecutor();
            executor.StartAttack(_ => _destroyable.DestroyLater());
        }
    }
}