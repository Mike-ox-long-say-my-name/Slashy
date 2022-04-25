namespace Core.Attacking.Interfaces
{
    public interface IMonoAttackAnimationEventDispatcher
    {
        void Register(IAttackAnimationEventReceiver receiver);
        void Unregister(IAttackAnimationEventReceiver receiver);
    }
}