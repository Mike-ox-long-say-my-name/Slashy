namespace Characters.Enemies.WeakHollow
{
    public class WeakHollowAttack : WeakHollowBaseState
    {
        public override void EnterState()
        {
            //Context.VelocityMovement.Stop();
            Context.AnimatorComponent.SetTrigger("attack");
            Context.PunchAttackExecutor.StartAttack(result =>
            {
                if (result.WasInterrupted)
                {
                    return;
                }

                if (result.Hits.Count > 0)
                {
                    SwitchState<WeakHollowRetreat>();
                }
                else
                {
                    SwitchState<WeakHollowPursue>();
                }
            });
        }
    }
}