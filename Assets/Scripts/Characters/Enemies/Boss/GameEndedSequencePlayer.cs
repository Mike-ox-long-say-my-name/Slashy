using System;
using System.Collections;
using Core;
using UnityEngine;

namespace Characters.Enemies.Boss
{
    public class GameEndedSequencePlayer : IGameEndedSequencePlayer
    {
        private const float WaitBeforeBlackoutTime = 5;
        private const float BlackoutTime = 5f;

        private readonly ICoroutineRunner _coroutineRunner;
        private readonly IBlackScreenService _blackScreenService;

        public event Action GameEndSequenceEnded;

        public GameEndedSequencePlayer(ICoroutineRunner coroutineRunner, IBlackScreenService blackScreenService)
        {
            _coroutineRunner = coroutineRunner;
            _blackScreenService = blackScreenService;
        }

        public void Play()
        {
            _coroutineRunner.Run(GameEndSequenceRoutine());
        }

        private IEnumerator GameEndSequenceRoutine()
        {
            yield return new WaitForSeconds(WaitBeforeBlackoutTime);

            _blackScreenService.Blackout(BlackoutTime);

            yield return new WaitForSeconds(BlackoutTime);
            GameEndSequenceEnded?.Invoke();
        }
    }
}