using Core.Attacking.Interfaces;
using UnityEngine;

namespace Core.Attacking.Mono
{
    public class MonoDotAttackHandler : MonoBehaviour
    {
        private IDotAttackExecutor _executor;

        public IDotAttackExecutor Executor
        {
            get
            {
                if (_executor != null)
                {
                    return _executor;
                }

                var attackBox = GetComponentInChildren<MonoDotAttackbox>().Attackbox;
                _executor = new DotAttackExecutor(attackBox);
                return _executor;
            }
        }
    }
}