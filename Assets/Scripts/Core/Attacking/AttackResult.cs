using System.Collections.Generic;
using System.Linq;
using Core.Attacking.Interfaces;

namespace Core.Attacking
{
    public class AttackResult
    {
        public bool WasInterrupted => !WasCompleted;
        public bool WasCompleted { get; }
        private readonly IHurtbox[] _hits;

        public IReadOnlyList<IHurtbox> Hits => _hits;

        public AttackResult(IEnumerable<IHurtbox> hits, bool wasCompleted)
        {
            _hits = hits.ToArray();
            WasCompleted = wasCompleted;    
        }
    }
}