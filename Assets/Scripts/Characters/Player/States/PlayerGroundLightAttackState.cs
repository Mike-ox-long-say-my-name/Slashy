﻿using Core.Attacking;

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
            Context.Movement.Stop();

            Context.AnimatorComponent.SetTrigger("attack");
            Context.Character.SpendStamina(Context.PlayerConfig.LightAttackFirstStaminaCost);

            _currentAttack = 1;
            _shouldContinueAttack = false;

            Context.LightMonoAttackFirst.StartAttack(AttackEndedFirst);
        }

        private void AttackEndedFirst(bool interrupted)
        {
            if (!interrupted && _shouldContinueAttack)
            {
                _currentAttack = 2;
                Context.Character.SpendStamina(Context.PlayerConfig.LightAttackSecondStaminaCost);
                Context.LightMonoAttackSecond.StartAttack(AttackEndedSecond);
            }
            else
            {
                AttackEndedSecond(interrupted);
            }
        }

        public override void UpdateState()
        {
            if (!_shouldContinueAttack && Context.Character.HasStamina() && Context.Input.IsLightAttackPressed)
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

        private void InterruptCurrentAttack()
        {
            switch (_currentAttack)
            {
                case 1:
                    Context.LightMonoAttackFirst.InterruptAttack();
                    break;
                case 2:
                    Context.LightMonoAttackSecond.InterruptAttack();
                    break;
            }
        }

        public override void OnDeath(HitInfo info)
        {
            InterruptCurrentAttack();
            base.OnDeath(info);
        }

        public override void OnStaggered(HitInfo info)
        {
            InterruptCurrentAttack();
            base.OnStaggered(info);
        }
    }
}