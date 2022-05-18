using System.Collections;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using Random = UnityEngine.Random;

namespace Effects
{
    public class TorchLingering : MonoBehaviour
    {
        [SerializeField] private Light2D light2d;
        [SerializeField] private float maxResize = 2f;

        private void Start()
        {
            StartNextRoutine();
        }

        private IEnumerator ResizeRoutine(float time, float resizeAmount, int resizeSign)
        {
            var passedTime = 0f;
            var scaledResizeAmount = resizeSign * resizeAmount / time;

            while (passedTime < time)
            {
                var step = Time.deltaTime;
                light2d.pointLightInnerRadius += step * scaledResizeAmount;

                passedTime += step;
                yield return null;
            }
        }

        private IEnumerator OneCycleRoutine(float time, float resizeAmount)
        {
            yield return ResizeRoutine(time, resizeAmount, 1);
            yield return new WaitForSeconds(Random.Range(0.1f, 0.3f));
            yield return ResizeRoutine(time, resizeAmount, -1);
            StartNextRoutine();
        }

        private void StartNextRoutine()
        {
            var time = Random.Range(0.4f, 3f);
            var resize = Random.Range(0, maxResize);
            StartCoroutine(OneCycleRoutine(time, resize));
        }
    }
}