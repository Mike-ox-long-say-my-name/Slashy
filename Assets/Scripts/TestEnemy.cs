using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TestEnemy : Character
{
    [SerializeField] private AttackExecutor attackExecutor;

    protected override void Awake()
    {
        base.Awake();
        InvokeRepeating(nameof(DoAttack), 0, 2);
    }

    private void DoAttack()
    {
        if (!attackExecutor.IsAttacking)
        {
            attackExecutor.StartExecution(this, null);
        }
    }
}
