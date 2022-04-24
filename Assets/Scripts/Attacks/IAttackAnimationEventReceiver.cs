namespace Attacks
{
    public interface IAttackAnimationEventReceiver
    {
        void OnEnableHitbox();
        void OnDisableHitbox();
        void OnAnimationEnded();
    }
}