namespace Core.Attacking.Interfaces
{
    public interface IDotAttackExecutor
    {
        bool IsDotEnabled { get; }

        void EnableDot();
        void DisableDot();
    }
}