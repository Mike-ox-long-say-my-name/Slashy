namespace Attacks
{
    public interface IMonoAttackAnimationEventDispatcher
    {
        void Register(IAttackAnimationEventReceiver receiver);
        void Unregister(IAttackAnimationEventReceiver receiver);
    }
}