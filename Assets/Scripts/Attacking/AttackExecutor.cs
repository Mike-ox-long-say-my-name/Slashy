using System;
using System.Collections;
using UnityEngine;

public abstract class AttackExecutor : MonoBehaviour
{
    private Coroutine _activeAttack;

    [SerializeField] private AttackHitbox hitbox;

    public AttackHitbox Hitbox => hitbox;
    public bool IsAttacking => _activeAttack != null;

    public void InterruptAttack()
    {
        OnAttackInterrupted();
        StopCoroutine(_activeAttack);
        _activeAttack = null;
    }

    public void StartExecution(IHitSource source, Action attackCompleted)
    {
        if (IsAttacking)
        {
            InterruptAttack();
        }
        _activeAttack = StartCoroutine(ExecuteInternal(source, attackCompleted));
    }

    private IEnumerator ExecuteInternal(IHitSource source, Action attackCompleted)
    {
        yield return Execute(source);
        attackCompleted?.Invoke();
        hitbox.DisableAndClear();
        _activeAttack = null;
    }

    protected abstract IEnumerator Execute(IHitSource source);

    protected virtual void OnAttackInterrupted()
    {
        hitbox.DisableAndClear();
    }
}

public class AttackInfo : ScriptableObject
{
    [Min(0)] public float baseDamageMultiplier = 1;
    [Min(0)] public float flatDamageBonus;
}

public struct HitInfo
{
    public float Damage;
    public float Balance;
}

public class PlayerAttackInfo : AttackInfo
{
    [Min(0)] public float staminaCost;
    [Min(0)] public float healthCost;
}