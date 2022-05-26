using System.Collections;
using UnityEngine;

namespace Core
{
    public class BlackScreenService : IBlackScreenService
    {
        private readonly BlackScreenServiceContainer _container;
        private readonly CoroutineContext _alphaChangeRoutine;

        public BlackScreenService(BlackScreenServiceContainer container, IGameLoader gameLoader,
            ICoroutineRunner coroutineRunner)
        {
            _container = container;
            _alphaChangeRoutine = coroutineRunner.GetEmptyContext();

            SubscribeToGameLoaderEvents();

            void SubscribeToGameLoaderEvents()
            {
                gameLoader.Exiting += _ => StopAlphaChange();
                gameLoader.StartingNewGame += _ => SetScreenAlpha(1);
                gameLoader.LoadingExitedLevel += _ => SetScreenAlpha(1);
                gameLoader.GameCompleted += () => SetScreenAlpha(0);
            }
        }

        private void StopAlphaChange()
        {
            _alphaChangeRoutine?.Stop();
        }

        public void Blackout(float time)
        {
            SetScreenAlpha(0);
            _alphaChangeRoutine.Restart(LerpScreenAlphaRoutine(1, time));
        }

        public void Whiteout(float time)
        {
            SetScreenAlpha(1);
            _alphaChangeRoutine.Restart(LerpScreenAlphaRoutine(_container.Threshold, time));
        }

        private void SetScreenAlpha(float alpha)
        {
            var color = _container.ScreenImage.color;
            color.a = alpha;
            _container.ScreenImage.color = color;
        }

        private IEnumerator LerpScreenAlphaRoutine(float targetAlpha, float time)
        {
            var blackScreen = _container.ScreenImage;

            var difference = targetAlpha - blackScreen.color.a;
            var passedTime = 0f;
            while (passedTime < time)
            {
                var deltaTime = Time.deltaTime;
                passedTime += deltaTime;
                SetScreenAlpha(blackScreen.color.a + difference * Time.deltaTime);
                yield return null;
            }

            if (targetAlpha <= _container.Threshold)
            {
                SetScreenAlpha(0);
            }
        }
    }
}