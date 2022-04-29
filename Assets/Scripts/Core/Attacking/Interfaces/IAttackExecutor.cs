using System;

namespace Core.Attacking.Interfaces
{
    public interface IAttackExecutor
    {
        bool IsAttacking { get; }

        void InterruptAttack();
        void StartAttack(Action<bool> attackEnded);
    }
}