namespace Core.Attacking.Interfaces
{
    public interface IAttackEventHandler
    {
        void HandleTick(IAnimationAttackExecutorContext context);
        void HandleEnableHitbox(IAnimationAttackExecutorContext context);
        void HandleDisableHitbox(IAnimationAttackExecutorContext context);
        void HandleAnimationEnd(IAnimationAttackExecutorContext context);
        void HandleAttackEnd(IAnimationAttackExecutorContext context, bool interrupted);
    }
}