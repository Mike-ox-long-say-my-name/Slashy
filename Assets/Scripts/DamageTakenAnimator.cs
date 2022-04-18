using System.Collections;
using Attacking;
using UnityEngine;

public class DamageTakenAnimator : MonoBehaviour
{
    [Header("Animation")]
    [SerializeField, Min(0)] private float flashTime;
    [SerializeField, Range(0, 1)] private float minTransparency;
    [SerializeField, Range(0, 1)] private float maxTransparency = 1;
    [SerializeField] private Color baseFlashColor = Color.white;
    [SerializeField] private Color zeroHpColor = Color.red;
    [SerializeField] private bool changeColorBasedOnHealth = true;

    [Space]
    [Header("Components")]
    [SerializeField] private SpriteRenderer maskSpriteRenderer;
    [SerializeField] private SpriteMask spriteMask;
    [SerializeField] private SpriteRenderer animationSpriteRenderer;
    
    private Coroutine _animationRoutine;

    public void OnHitReceived(HittableEntity entity, HitInfo info)
    {
        if (_animationRoutine != null)
        {
            StopCoroutine(_animationRoutine);
        }

        IEnumerator AnimationRoutine()
        {
            var lastStepTime = Time.time;
            var passedTime = 0f;

            while (passedTime < flashTime)
            {
                spriteMask.sprite = maskSpriteRenderer.sprite;

                passedTime += Time.time - lastStepTime;
                lastStepTime = Time.time;
                var fraction = passedTime / flashTime;

                var color = animationSpriteRenderer.color;
                if (changeColorBasedOnHealth && entity is Character character)
                {
                    var healthFraction = character.Health.Value / character.Health.MaxValue;
                    color = Color.Lerp(baseFlashColor, zeroHpColor, 1 - healthFraction);
                }

                color.a = minTransparency + maxTransparency * (1 - minTransparency) * (1 - fraction);

                animationSpriteRenderer.color = color;

                yield return new WaitForEndOfFrame();
            }

            var fullTransparencyColor = baseFlashColor;
            fullTransparencyColor.a = 0;
            animationSpriteRenderer.color = fullTransparencyColor;
        }

        if (_animationRoutine != null)
        {
            StopCoroutine(_animationRoutine);
        }
        _animationRoutine = StartCoroutine(AnimationRoutine());
    }
}
