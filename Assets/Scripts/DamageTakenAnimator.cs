using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTakenAnimator : MonoBehaviour
{
    [SerializeField] private HittableEntity entity;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField, Min(0)] private float fullWhiteOutTime;

    private Coroutine _animationRoutine;

    private void Awake()
    {
        entity.OnHitReceived.AddListener(OnHitReceived);
    }

    private void OnHitReceived(HittableEntity _)
    {
        if (_animationRoutine != null)
        {
            StopCoroutine(_animationRoutine);
        }

        IEnumerator AnimationRoutine()
        {
            yield break;
        }

        _animationRoutine = StartCoroutine(AnimationRoutine());
    }
}
