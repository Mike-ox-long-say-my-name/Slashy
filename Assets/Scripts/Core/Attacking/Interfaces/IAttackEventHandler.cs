namespace Core.Attacking
{
    public interface IAttackEventHandler
    {
        void HandleTick(IAnimationAttackExecutorContext context);
        void HandleEnableHitbox(IAnimationAttackExecutorContext context);
        void HandleDisableHitbox(IAnimationAttackExecutorContext context);
        void HandleAnimationEnd(IAnimationAttackExecutorContext context);
    }
}