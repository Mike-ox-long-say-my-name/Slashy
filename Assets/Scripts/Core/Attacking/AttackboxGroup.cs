using System.Collections.Generic;
using Core.Attacking.Interfaces;

namespace Core.Attacking
{
    public class AttackboxGroup
    {
        private readonly HashSet<IHurtbox> _hits = new HashSet<IHurtbox>();

        public bool TryAttack(IHurtbox hit)
        {
            return _hits.Add(hit);
        }

        public void Reset()
        {
            _hits.Clear();
        }
    }
}