using Core.Attacking.Mono;
using Core.Characters.Mono;
using UnityEngine;

namespace Characters.Enemies.Examples
{
    public class TestEnemy : MixinCharacter
    {
        [SerializeField] private MonoAttackHandler monoAttackHandler;

        private void Awake()
        {
            InvokeRepeating(nameof(DoAttack), 0, 2);
        }

        private void DoAttack()
        {
            if (!monoAttackHandler.Executor.IsAttacking)
            {
                monoAttackHandler.Executor.StartAttack(null);
            }
        }
    }
}
