using Core;
using Core.Attacking.Mono;
using Core.Characters.Mono;
using UnityEngine;

namespace Characters.Enemies.Boss
{
    [RequireComponent(typeof(MixinDamageSource))]
    [RequireComponent(typeof(DestroyHelper))]
    public class BossSpike : MonoBehaviour
    {
        [SerializeField] private MonoAbstractAttackExecutor strikeAttackExecutor;

        private DestroyHelper _destroyable;

        private void Awake()
        {
            _destroyable = GetComponent<DestroyHelper>();
        }

        public void StrikeFromGround()
        {
            var executor = strikeAttackExecutor.GetExecutor();
            executor.StartAttack(_ => _destroyable.DestroyLater());
        }
    }
}