using UnityEngine;

namespace Core.Attacking
{
    public abstract class MonoBaseHitbox : MonoBehaviour
    {
        [SerializeField] private Collider[] colliders;

        protected Collider[] Colliders => colliders;
    }
}