using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Hitbox : MonoBehaviour
{
    [SerializeField] protected Collider hitboxCollider;


    private void OnTriggerEnter(Collider other)
    {
        OnHit(other);
    }

    protected virtual void OnHit(Collider target)
    {
    }

    public void Enable()
    {
        hitboxCollider.enabled = true;
    }

    public void Disable()
    {
        hitboxCollider.enabled = false;
    }
}
