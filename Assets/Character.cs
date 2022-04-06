using System;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] private float maxHealth;

    public float Health { get; private set; }

    private void Awake()
    {
        Health = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        if (damage < 0)
        {
            throw new ArgumentException("Damage can't be less than zero");
        }
        Health = Mathf.Max(0, Health - damage);
        if (Health == 0)
        {
            OnDeath();
        }
    }

    protected virtual void OnDeath()
    {
        Destroy(gameObject);
    }
}