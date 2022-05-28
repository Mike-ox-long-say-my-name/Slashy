using System;
using System.Collections;
using UnityEngine;

namespace Core
{
    public class PlayerDeathSequencePlayer : IPlayerDeathSequencePlayer
    {
        public event Action DeathSequenceStarted;
        public event Action DeathSequenceEnded;
        
        private readonly IBlackScreenService _blackScreenService;
        private readonly CoroutineContext _sequenceRoutine;

        private const float BlackoutTime = 3;
        private const float SlowTimeDuration = 1.5f;
        private const float MinTimeScale = 0.4f;


        public PlayerDeathSequencePlayer(ICoroutineRunner coroutineRunner, IBlackScreenService blackScreenService,
            IGameLoader gameLoader)
        {
            _sequenceRoutine = coroutineRunner.GetEmptyContext();
            _blackScreenService = blackScreenService;
            gameLoader.Exiting += OnExiting;
        }

        private void OnExiting(string _)
        {
            _sequenceRoutine.Stop();
            Time.timeScale = 1;
        }

        public void Play()
        {
            _sequenceRoutine.Start(DeathSequenceRoutine());
        }

        private IEnumerator DeathSequenceRoutine()
        {
            DeathSequenceStarted?.Invoke();
            
            yield return SlowTimeTemporary(SlowTimeDuration, MinTimeScale);
            _blackScreenService.Blackout(BlackoutTime);
            yield return new WaitForSeconds(BlackoutTime);
            
            DeathSequenceEnded?.Invoke();
        }

        private static IEnumerator SlowTimeTemporary(float time, float target)
        {
            var halfTime = time / 2;
            var speed = (Time.timeScale - target) / halfTime;
            var passedTime = 0f;
            var sign = -1;
            while (passedTime < time)
            {
                var deltaTime = Time.unscaledDeltaTime;
                passedTime += deltaTime;

                if (sign == -1 && passedTime >= halfTime)
                {
                    Time.timeScale = target;
                    sign = 1;
                }

                Time.timeScale += sign * deltaTime * speed;
                yield return null;
            }

            Time.timeScale = 1;
        }
    }
}