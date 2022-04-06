using System;
using System.Collections;
using UnityEngine;

public abstract class AttackExecutor : MonoBehaviour
{
    private Coroutine _activeAttack;

    public void StartExecution(Action attackCompleted)
    {
        StopCoroutine(_activeAttack);
        _activeAttack = StartCoroutine(ExecuteInternal(attackCompleted));
    }

    private IEnumerator ExecuteInternal(Action attackCompleted)
    {
        yield return Execute();
        attackCompleted();
    }

    protected abstract IEnumerator Execute();
}

public class AttackInfo : ScriptableObject
{
    [Min(0)] public float baseDamageMultiplier = 1;
    [Min(0)] public float flatDamageBonus;
}

public class PlayerAttackInfo : AttackInfo
{
    [Min(0)] public float staminaCost;
    [Min(0)] public float healthCost;
}