using Core.Characters.Interfaces;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    [DefaultExecutionOrder(-1)]
    public class ResourceBar : MonoBehaviour
    {
        [SerializeField] private bool firstChangeNoAnimation = true;
        [SerializeField] private Image frontLayerImage;
        [SerializeField] private Image middleLayerImage;
        [SerializeField, Range(0, 0.1f)] private float fillAnimationPerTick = 0.05f;
        [SerializeField, Min(0)] private float fillAnimationTickInterval = 0.05f;
        [SerializeField, Range(0, 1)] private float fillAnimationStartDelay = 0.15f;

        private bool _firstAnimationHappened;
        private float _maxFill;

        private void Awake()
        {
            _maxFill = frontLayerImage.fillAmount;
        }

        private Coroutine _animationRoutine;

        public void OnResourceValueChanged(IResource resource)
        {
            if (_animationRoutine != null)
            {
                StopCoroutine(_animationRoutine);
            }

            var fraction = resource.Value / resource.MaxValue * _maxFill;

            if (!_firstAnimationHappened && firstChangeNoAnimation)
            {
                middleLayerImage.fillAmount = fraction;
                frontLayerImage.fillAmount = fraction;
                _firstAnimationHappened = true;
                return;
            }

            if (fraction < middleLayerImage.fillAmount)
            {
                middleLayerImage.fillAmount = frontLayerImage.fillAmount;
                frontLayerImage.fillAmount = fraction;
                _animationRoutine =
                    StartCoroutine(MiddleLayerAnimation(middleLayerImage, 
                        fraction, fillAnimationStartDelay));
            }
            else
            {
                frontLayerImage.fillAmount = middleLayerImage.fillAmount;
                middleLayerImage.fillAmount = fraction;
                _animationRoutine =
                    StartCoroutine(MiddleLayerAnimation(frontLayerImage, 
                        fraction, 0));
            }
        }

        private IEnumerator MiddleLayerAnimation(Image target, float targetFill, float delay)
        {
            yield return new WaitForSeconds(delay);

            var fill = target.fillAmount;
            var sign = Mathf.Sign(targetFill - fill);
            while (!Mathf.Approximately(fill, targetFill))
            {
                fill = Mathf.Clamp(fill + fillAnimationPerTick * sign, Mathf.Min(fill, targetFill),
                    Mathf.Max(fill, targetFill));
                target.fillAmount = fill;
                yield return new WaitForSeconds(fillAnimationTickInterval);
            }
        }
    }
}