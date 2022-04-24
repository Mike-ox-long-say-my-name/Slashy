using System;

namespace Attacks
{
    public static class AttackExecutorExtensions
    {
        private class AttackEndEventReceiver : IAttackEndEventReceiver
        {
            public Action<bool> AttackEnded { get; set; }

            public void OnAttackEnded(bool interrupted)
            {
                AttackEnded?.Invoke(interrupted);
            }
        }

        public static void StartAttack(this IAttackExecutor executor, Action<bool> attackEnded)
        {
            executor.StartAttack(new AttackEndEventReceiver
            {
                AttackEnded = attackEnded
            });
        }
    }
}