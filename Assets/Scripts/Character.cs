using Attacking;
using System;
using UnityEngine;
using UnityEngine.Events;

public class Character : HittableEntity, IHitSource
{
    [field: SerializeField]
    public UnityEvent<ICharacterResource> OnHealthChanged { get; private set; }
        = new UnityEvent<ICharacterResource>();

    [SerializeField, Min(0)] private float maxHealth;
    [SerializeField] private bool canDie = true;

    private HealthResource _healthResource;
    public ICharacterResource Health => _healthResource;

    public Transform Transform => transform;
    public HittableEntity Source => this;

    protected virtual void Awake()
    {
        _healthResource = new HealthResource(this, maxHealth);
    }

    public override void ReceiveHit(IHitSource source, in HitInfo info)
    {
        var damage = info.damage;
        if (damage < 0)
        {
            throw new ArgumentException();
        }

        _healthResource.Spend(damage);
        OnHealthChanged?.Invoke(Health);

        base.ReceiveHit(source, info);

        if (canDie && _healthResource.IsDepleted)
        {
            OnDeath();
        }
    }

    protected virtual void OnDeath()
    {
        Destroy(gameObject);
    }
}