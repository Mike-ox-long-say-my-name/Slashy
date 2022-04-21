namespace Characters.Enemies.States
{
    public interface IStateHolder<TContext>
    {
        EnemyBaseState<TContext> CurrentState { get; set; }
    }
}