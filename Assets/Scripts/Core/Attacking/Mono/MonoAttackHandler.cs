using Core.Attacking.Interfaces;
using Core.Utilities;
using UnityEngine;

namespace Core.Attacking.Mono
{
    [DisallowMultipleComponent]
    public abstract class MonoAttackHandler : MonoBehaviour
    {
        private IAttackExecutor _executor;

        protected abstract IAttackExecutor CreateExecutor(ICoroutineHost host, IAttackbox attackbox);

        public IAttackExecutor Executor
        {
            get
            {
                if (_executor != null)
                {
                    return _executor;
                }

                var attackBox = GetComponentInChildren<MonoAttackbox>().Attackbox;
                _executor = CreateExecutor(this.ToCoroutineHost(), attackBox);

                return _executor;
            }
        }
    }
}