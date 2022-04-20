using Core.Characters;

namespace Characters.Player.States
{
    public class PlayerGroundLightAttackState : PlayerBaseGroundedState
    {
        public PlayerGroundLightAttackState(PlayerStateMachine context, PlayerStateFactory factory) : base(context, factory)
        {
        }

        private int _currentAttack;
        private bool _shouldContinueAttack;

        public override void EnterState()
        {
            Context.AnimatorComponent.SetTrigger("attack");

            Context.Movement.ResetXZVelocity();
            Context.PlayerCharacter.SpendStamina(Context.PlayerConfig.LightAttackFirstStaminaCost);

            _currentAttack = 1;
            _shouldContinueAttack = false;

            Context.LightAttackFirst.StartExecution(Context.PlayerCharacter, AttackEndedFirst);
        }

        private void AttackEndedFirst(bool interrupted)
        {
            if (!interrupted && _shouldContinueAttack)
            {
                _currentAttack = 2;
                Context.PlayerCharacter.SpendStamina(Context.PlayerConfig.LightAttackSecondStaminaCost);
                Context.LightAttackSecond.StartExecution(Context.PlayerCharacter, AttackEndedSecond);
            }
            else
            {
                AttackEndedSecond(interrupted);
            }
        }

        public override void UpdateState()
        {
            if (!_shouldContinueAttack && Context.PlayerCharacter.HasStamina && Context.Input.IsLightAttackPressed.CheckAndReset())
            {
                Context.AnimatorComponent.SetTrigger("attack");
                _shouldContinueAttack = true;
            }
        }

        private void AttackEndedSecond(bool interrupted)
        {
            if (!interrupted)
            {
                SwitchState(Factory.Grounded());
            }
            else
            {
                Context.AnimatorComponent.ResetTrigger("attack");
            }
        }

        public override void InterruptState(CharacterInterruption interruption)
        {
            switch (_currentAttack)
            {
                case 1:
                    Context.LightAttackFirst.InterruptAttack();
                    break;
                case 2:
                    Context.LightAttackSecond.InterruptAttack();
                    break;
            }
            base.InterruptState(interruption);
        }
    }
}