namespace Characters.Enemies.States
{
    public interface IStateHolder<TContext> where TContext : class, IStateHolder<TContext>
    {
        EnemyBaseState<TContext> CurrentState { get; set; }
    }
}