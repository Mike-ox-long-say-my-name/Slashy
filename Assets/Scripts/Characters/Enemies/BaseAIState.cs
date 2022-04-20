namespace Characters.Enemies
{
    public class BaseAIState
    {
        protected virtual void UpdateState()
        {
        }

        protected virtual void EnterState()
        {
        }

        protected virtual void ExitState()
        {
        }

        protected virtual void CheckStateSwitch()
        {
        }

        protected void SwitchState(BaseAIState state)
        {
        }
    }
}