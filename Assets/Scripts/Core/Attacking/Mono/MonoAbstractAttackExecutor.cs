using Core.Attacking.Interfaces;
using UnityEngine;

namespace Core.Attacking.Mono
{

    [DisallowMultipleComponent]
    public abstract class MonoAbstractAttackExecutor : MonoBehaviour
    {
        public abstract IAttackExecutor GetExecutor();
    }
}