using UnityEngine;

[RequireComponent(typeof(Collider))]
public class BaseHitbox : MonoBehaviour
{
    [SerializeField] protected Collider hitboxCollider;

    public virtual void Enable()
    {
        hitboxCollider.enabled = true;
    }

    public virtual void Disable()
    {
        hitboxCollider.enabled = false;
    }
}
