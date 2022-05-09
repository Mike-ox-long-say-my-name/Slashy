using Core.Attacking.Interfaces;

namespace Core.Attacking
{
    public interface IAnimationAttackExecutorContext
    {
        void EndAttack();
        IAttackbox Attackbox { get; }
    }
}