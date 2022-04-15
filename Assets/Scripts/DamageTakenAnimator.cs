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
            var lastStepTime = Time.time;
            var passedTime = 0f;
            while (passedTime < fullWhiteOutTime)
            {
                var timeStep = Time.time - lastStepTime;
                var fraction = passedTime / fullWhiteOutTime;

                var color = spriteRenderer.color;
                color.a = fraction;
                spriteRenderer.color = color;

                yield return new WaitForEndOfFrame();
            }
        }

        if (_animationRoutine != null)
        {
            StopCoroutine(_animationRoutine);
        }
        _animationRoutine = StartCoroutine(AnimationRoutine());
    }
}
