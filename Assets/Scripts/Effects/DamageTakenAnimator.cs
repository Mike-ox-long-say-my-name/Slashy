using Core.Attacking;
using System.Collections;
using Core.Attacking.Interfaces;
using Core.Characters.Interfaces;
using UnityEngine;

namespace Effects
{
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
        
        private void OnEnable()
        {
            var character = GetComponentInParent<IMonoCharacter>();
            character?.OnHitReceived.AddListener(OnHitReceived);
        }

        private void OnDisable()
        {
            var character = GetComponentInParent<IMonoCharacter>();
            character?.OnHitReceived.RemoveListener(OnHitReceived);
        }

        public void OnHitReceived(IHitReceiver entity, HitInfo info)
        {
            if (_animationRoutine != null)
            {
                StopCoroutine(_animationRoutine);
            }

            if (_animationRoutine != null)
            {
                StopCoroutine(_animationRoutine);
            }
            _animationRoutine = StartCoroutine(AnimationRoutine(entity));
        }

        private IEnumerator AnimationRoutine(IHitReceiver entity)
        {
            var passedTime = 0f;

            while (passedTime < flashTime)
            {
                spriteMask.sprite = maskSpriteRenderer.sprite;

                passedTime += Time.deltaTime;
                var fraction = passedTime / flashTime;

                var color = animationSpriteRenderer.color;
                if (changeColorBasedOnHealth && entity is ICharacter character)
                {
                    var healthFraction = character.Health.Value / character.Health.MaxValue;
                    color = Color.Lerp(baseFlashColor, zeroHpColor, 1 - healthFraction);
                }

                color.a = minTransparency + maxTransparency * (1 - minTransparency) * (1 - fraction);

                animationSpriteRenderer.color = color;

                yield return null;
            }

            var fullTransparencyColor = baseFlashColor;
            fullTransparencyColor.a = 0;
            animationSpriteRenderer.color = fullTransparencyColor;
        }
    }
}
