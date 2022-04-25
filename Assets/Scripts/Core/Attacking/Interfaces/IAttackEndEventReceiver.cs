namespace Core.Attacking.Interfaces
{
    public interface IAttackEndEventReceiver
    {
        void OnAttackEnded(bool interrupted);
    }
}