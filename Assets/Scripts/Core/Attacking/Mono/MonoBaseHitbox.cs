using UnityEngine;

namespace Core.Attacking.Mono
{
    public abstract class MonoBaseHitbox : MonoBehaviour
    {
        [SerializeField] private Collider[] colliders;

        protected Collider[] Colliders => colliders;
    }
}