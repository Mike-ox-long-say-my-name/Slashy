using Core.Characters.Interfaces;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ResourceBar : MonoBehaviour
    {
        [SerializeField] private Image frontLayerImage;
        [SerializeField] private Image middleLayerImage;
        [SerializeField, Range(0, 0.1f)] private float fillAnimationPerTick = 0.05f;
        [SerializeField, Min(0)] private float fillAnimationTickInterval = 0.05f;
        [SerializeField, Range(0, 1)] private float fillAnimationStartDelay = 0.15f;

        public void OnResourceValueChanged(ICharacterResource resource)
        {
            var fraction = resource.Value / resource.MaxValue;
            if (_middleLayerAnimation != null)
            {
                StopCoroutine(_middleLayerAnimation);
            }

            middleLayerImage.fillAmount = frontLayerImage.fillAmount;
            frontLayerImage.fillAmount = fraction;

            _middleLayerAnimation = StartCoroutine(MiddleLayerAnimation(fraction));
        }

        private IEnumerator MiddleLayerAnimation(float targetFill)
        {
            if (Mathf.Approximately(fillAnimationPerTick, 0))
            {
                yield break;
            }

            if (fillAnimationStartDelay > 0)
            {
                yield return new WaitForSeconds(fillAnimationStartDelay);
            }

            var fill = middleLayerImage.fillAmount;
            while (fill > targetFill)
            {
                fill = Mathf.Max(targetFill, fill - fillAnimationPerTick);
                middleLayerImage.fillAmount = fill;
                yield return new WaitForSeconds(fillAnimationTickInterval);
            }
        }

        private Coroutine _middleLayerAnimation;
    }
}