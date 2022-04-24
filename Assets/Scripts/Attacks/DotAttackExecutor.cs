using Core.Attacking;
using Core.Characters;

namespace Attacks
{
    public class DotAttackExecutor : IDotAttackExecutor
    {
        protected IDotAttackbox Attackbox { get; }

        public DotAttackExecutor(IDotAttackbox attackbox)
        {
            Guard.NotNull(attackbox);

            Attackbox = attackbox;
        }

        public bool IsDotEnabled => Attackbox.IsEnabled;

        public void EnableDot()
        {
            Attackbox.Enable();
        }

        public void DisableDot()
        {
            Attackbox.Disable();
        }
    }
}