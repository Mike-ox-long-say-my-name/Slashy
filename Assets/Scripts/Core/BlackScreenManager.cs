using System.Collections;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace Core
{
    public class BlackScreenManager : PublicSingleton<BlackScreenManager>
    {
        [SerializeField] private Image blackScreen;
        [SerializeField, Min(0)] private float threshold;
        [field: SerializeField] public float DefaultTime { get; [UsedImplicitly] private set; } = 1.5f;

        private void Start()
        {
            GameLoader.Instance.Exiting.AddListener(_ =>
            {
                StopAllCoroutines();
            });
            GameLoader.Instance.StartingNewGame.AddListener(_ => SetScreenAlpha(1));
            GameLoader.Instance.LoadingExitedLevel.AddListener(_ => SetScreenAlpha(1));
            GameLoader.Instance.GameCompleted.AddListener(() => SetScreenAlpha(0));
        }

        public void Blackout(float time)
        {
            StopAllCoroutines();
            SetScreenAlpha(0);
            StartCoroutine(LerpScreenAlphaRoutine(1, time));
        }

        public void Whiteout(float time)
        {
            StopAllCoroutines();
            SetScreenAlpha(1);
            StartCoroutine(LerpScreenAlphaRoutine(threshold, time));
        }

        private void SetScreenAlpha(float alpha)
        {
            var color = blackScreen.color;
            color.a = alpha;
            blackScreen.color = color;
        }

        private IEnumerator LerpScreenAlphaRoutine(float targetAlpha, float time)
        {
            var difference = targetAlpha - blackScreen.color.a;
            var passedTime = 0f;
            while (passedTime < time)
            {
                var deltaTime = Time.deltaTime;
                passedTime += deltaTime;
                SetScreenAlpha(blackScreen.color.a + difference * Time.deltaTime);
                yield return null;
            }

            if (targetAlpha <= threshold)
            {
                SetScreenAlpha(0);
            }
        }
    }
}