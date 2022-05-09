using Core.Attacking.Interfaces;
using UnityEngine;

namespace Core.Attacking.Mono
{
    public abstract class MonoBaseHitbox : MonoBehaviour
    {
        [SerializeField] protected Collider[] colliders;

        public abstract IHitbox Hitbox { get; }

    }
}