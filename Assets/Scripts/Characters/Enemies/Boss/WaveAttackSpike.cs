using System.Collections;
using Core;
using Core.Attacking;
using Core.Attacking.Interfaces;
using Core.Attacking.Mono;
using UnityEngine;

namespace Characters.Enemies.Boss
{
    [RequireComponent(typeof(MixinDestroyable))]
    public class WaveAttackSpike : MonoBehaviour
    {
        [SerializeField] private MonoAbstractAttackExecutor attackExecutor;
        [SerializeField] private float movePerSecond;
        [SerializeField] private float animationTime;
        [SerializeField] private Transform spike;

        private MixinDestroyable _destroyable;
        private IAttackbox _attackbox;

        private void Awake()
        {
            _destroyable = GetComponent<MixinDestroyable>();
            _attackbox = GetComponentInChildren<MonoAttackbox>().Attackbox;
        }

        public void StrikeFromGround(AttackboxGroup group)
        {
            _attackbox.Group = group;
            var executor = attackExecutor.GetExecutor();
            executor.StartAttack(OnAttackEnded);
            StartCoroutine(AnimationRoutine(spike, movePerSecond, animationTime));
        }

        private static IEnumerator AnimationRoutine(Transform target, float move, float time)
        {
            var passedTime = 0f;
            while (passedTime < time)
            {
                var deltaTime = Time.deltaTime;
                passedTime += deltaTime;
                var moveVector = new Vector3(0, move * deltaTime, 0);
                target.position += moveVector;

                yield return null;
            }
        }

        private void OnAttackEnded(AttackResult obj)
        {
            _destroyable.DestroyLater();
        }
    }
}