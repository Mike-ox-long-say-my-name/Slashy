namespace Attacks
{
    public interface IDotAttackExecutor
    {
        bool IsDotEnabled { get; }

        void EnableDot();
        void DisableDot();
    }
}