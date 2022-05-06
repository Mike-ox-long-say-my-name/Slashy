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
            StartNextRoutine(1);
        }

        private IEnumerator ResizeRoutine(float time, float resizeAmount, int resizeSign)
        {
            var passedTime = 0f;
            var scaledResizeAmount = resizeSign * resizeAmount / time;

            while (passedTime < time)
            {
                var step = Time.deltaTime;
                light2d.intensity += step * scaledResizeAmount;

                passedTime += step;
                yield return null;
            }

            StartNextRoutine(-resizeSign);
        }

        private void StartNextRoutine(int resizeSign)
        {
            var time = 2;
            var resize = Random.Range(-maxResize, maxResize);
            StartCoroutine(ResizeRoutine(time, resize, resizeSign));
        }
    }
}