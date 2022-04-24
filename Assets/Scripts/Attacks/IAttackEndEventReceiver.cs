namespace Attacks
{
    public interface IAttackEndEventReceiver
    {
        void OnAttackEnded(bool interrupted);
    }
}