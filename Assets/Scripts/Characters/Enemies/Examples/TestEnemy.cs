using Core.Attacking.Mono;
using Core.Characters.Mono;
using UnityEngine;

namespace Characters.Enemies.Examples
{
    public class TestEnemy : MonoCharacter
    {
        [SerializeField] private MonoAttackHandler monoAttackHandler;

        private void Awake()
        {
            InvokeRepeating(nameof(DoAttack), 0, 2);
        }

        private void DoAttack()
        {
            if (!monoAttackHandler.Resolve().IsAttacking)
            {
                monoAttackHandler.Resolve().StartAttack(null);
            }
        }
    }
}