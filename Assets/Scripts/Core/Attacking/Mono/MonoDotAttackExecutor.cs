using Core.Attacking.Interfaces;

namespace Core.Attacking.Mono
{
    public class MonoDotAttackExecutor : MonoAbstractAttackExecutor
    {
        private IAttackExecutor CreateExecutor()
        {
            var attackBox = GetComponentInChildren<MonoDotAttackbox>().Attackbox;
            return new DotAttackExecutor(attackBox);
        }

        private IAttackExecutor _executor;

        public override IAttackExecutor GetExecutor()
        {
            return _executor ??= CreateExecutor();
        }
    }
}