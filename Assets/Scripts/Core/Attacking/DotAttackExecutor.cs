using System;
using System.Collections.Generic;
using Core.Attacking.Interfaces;
using Core.Utilities;

namespace Core.Attacking
{
    public class DotAttackExecutor : IAttackExecutor
    {
        private readonly IDotAttackbox _attackbox;

        public DotAttackExecutor(IDotAttackbox attackbox)
        {
            Guard.NotNull(attackbox);
            _attackbox = attackbox;
        }

        public bool IsAttacking => _attackbox.IsEnabled;

        public void InterruptAttack()
        {
            _attackbox.Disable();
            _attackEnded?.Invoke(new AttackResult(_hits, false));
            _hits.Clear();
            _attackEnded = null;
        }

        private Action<AttackResult> _attackEnded;
        private readonly List<IHurtbox> _hits = new List<IHurtbox>();

        public void StartAttack(Action<AttackResult> attackEnded)
        {
            _attackEnded = attackEnded;
            _attackbox.Enable();
        }

        public void RegisterHit(IHurtbox hit)
        {
            _hits.Add(hit);
        }
    }
}