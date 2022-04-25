namespace Core.Attacking.Interfaces
{
    public interface IAttackExecutor
    {
        bool IsAttacking { get; }

        void InterruptAttack();
        void StartAttack(IAttackEndEventReceiver endReceiver);
        bool InterceptHit(IHurtbox hit);
    }
}