using System;
using Attacking;
using UnityEngine;

public class Character : HittableEntity, IHitSource
{
    [SerializeField, Min(0)] private float maxHealth;

    public float MaxHealth => maxHealth;
    public float Health { get; private set; }

    protected virtual void Awake()
    {
        Health = maxHealth;
    }

    public override void ReceiveHit(IHitSource source, in HitInfo info)
    {
        var damage = info.Damage;
        if (damage < 0)
        {
            throw new ArgumentException();
        }

        Health = Mathf.Max(0, Health - damage);
        if (Mathf.Abs(Health) < 1e-8)
        {
            OnDeath();
        }
    }

    protected virtual void OnDeath()
    {
        Destroy(gameObject);
    }

    public Transform Transform => transform;
    public HittableEntity Source => this;
}