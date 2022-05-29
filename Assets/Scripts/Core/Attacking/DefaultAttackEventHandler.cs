using Core.Attacking.Interfaces;

namespace Core.Attacking
{
    public class DefaultAttackEventHandler : IAttackEventHandler
    {
        public virtual void HandleTick(IAnimationAttackExecutorContext context)
        {
        }

        public virtual void HandleEnableHitbox(IAnimationAttackExecutorContext context)
        {
            context.Attackbox.Enable();
        }

        public virtual void HandleDisableHitbox(IAnimationAttackExecutorContext context)
        {
            context.Attackbox.Disable();
        }

        public virtual void HandleAnimationEnd(IAnimationAttackExecutorContext context)
        {
        }

        public virtual void HandleAttackEnd(IAnimationAttackExecutorContext context, bool interrupted)
        {
            context.Attackbox.Disable();
        }
    }
}