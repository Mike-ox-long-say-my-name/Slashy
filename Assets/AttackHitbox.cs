using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AttackHitbox : BaseHitbox
{
    [SerializeField] private UnityEvent<HittableHitbox> onHit;

    private Dictionary<int, HittableHitbox> _hitboxes = new Dictionary<int, HittableHitbox>();

    public void ClearHits()
    {
        _hitboxes.Clear();
    }

    protected override void OnHit(Collider target)
    {
        if (!target.TryGetComponent<HittableHitbox>(out var hit))
        {
            return;
        }

        var hitId = hit.gameObject.GetInstanceID();
        if (_hitboxes.ContainsKey(hitId))
        {
            return;
        }

        _hitboxes.Add(hitId, hit);

        onHit?.Invoke(hit);
    }
}
