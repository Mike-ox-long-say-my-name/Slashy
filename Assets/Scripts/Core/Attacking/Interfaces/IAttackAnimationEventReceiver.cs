namespace Core.Attacking.Interfaces
{
    public interface IAttackAnimationEventReceiver
    {
        void ReceiveEnableHitbox();
        void ReceiveDisableHitbox();
        void ReceiveAnimationEnded();
    }
}