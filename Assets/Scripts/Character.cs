using Attacking;
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

    public override void ReceiveHit(in HitInfo info)
    {
        _healthResource.Spend(info.DamageInfo.Damage);
        OnHealthChanged?.Invoke(Health);

        base.ReceiveHit(info);

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