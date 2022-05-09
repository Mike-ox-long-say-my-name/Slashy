namespace Core.Attacking.Interfaces
{
    public interface IAnimationAttackExecutorContext
    {
        void EndAttack();
        IAttackbox Attackbox { get; }
    }
}