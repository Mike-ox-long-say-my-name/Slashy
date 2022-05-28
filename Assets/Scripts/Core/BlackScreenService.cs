using System.Collections;
using UnityEngine;

namespace Core
{
    public class BlackScreenService : IBlackScreenService
    {
        private readonly BlackScreenServiceContainer _container;
        private readonly CoroutineContext _alphaChangeRoutine;

        public BlackScreenService(ICoroutineRunner coroutineRunner, IGameLoader gameLoader,
            BlackScreenServiceContainer container)
        {
            _container = container;
            _alphaChangeRoutine = coroutineRunner.GetEmptyContext();

            SubscribeToGameLoaderEvents();

            void SubscribeToGameLoaderEvents()
            {
                gameLoader.Exiting += _ => StopAlphaChange();
                gameLoader.StartingNewGame += _ => OnLoadedLevel();
                gameLoader.LoadingExitedLevel += _ => OnLoadedLevel();
                gameLoader.LoadedLevel += _ => OnLoadedLevel();
                gameLoader.GameCompleted += () => SetScreenAlpha(0);
            }
        }

        private void OnLoadedLevel()
        {
            SetScreenAlpha(1);
            Whiteout();
        }

        private void StopAlphaChange()
        {
            _alphaChangeRoutine?.Stop();
        }

        public void Blackout() => Blackout(_container.DefaultTime);

        public void Whiteout() => Whiteout(_container.DefaultTime);

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