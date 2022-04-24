using Attacks;
using Core.Characters;
using UnityEngine;

namespace Characters.Enemies
{
    public class TestEnemy : MonoCharacter
    {
        [SerializeField] private MonoAttackHandler monoAttackHandler;

        protected override void Awake()
        {
            base.Awake();
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
