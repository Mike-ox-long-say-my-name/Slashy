using Core.Characters.Interfaces;

namespace Characters.Player.States
{
    public class PlayerDeathState : PlayerBaseState
    {
        protected override void SwitchState<TState>(bool ignoreValidness = false)
        {
            // Запрещаем выход их этого состояния
        }

        public override void EnterState()
        {
            Context.Hurtbox.Disable();
            Context.VelocityMovement.Stop();
            Context.Animator.PlayDeathAnimation();
            Context.DeathSequencePlayer.Play();
        }
    }
}