namespace Attacks
{
    public interface IMonoAttackHandler
    {
        IAttackExecutor Executor { get; }
    }
}