using System;
using System.Collections;
using Core;
using UnityEngine;

namespace Characters.Player
{
    public class PlayerAwarenessEffect : MonoBehaviour
    {
        [SerializeField] private AudioSource source;
        [SerializeField] private GameObject dangerIcon;
        [SerializeField] private float appearTime;
        
        private IPlayerAwareness _playerAwareness;
        private CoroutineContext _showingIconRoutine;

        private void Awake()
        {
            _playerAwareness = Container.Get<IPlayerAwareness>();
            var runner = Container.Get<ICoroutineRunner>();
            _showingIconRoutine = runner.GetEmptyContext();
            _playerAwareness.Danger += OnDanger;
        }

        private void OnDestroy()
        {
            _showingIconRoutine.Stop();
        }

        private void OnDanger()
        {
            source.Play();
            _showingIconRoutine.Stop();
            _showingIconRoutine.Start(ShowIconTemporary());
        }

        private IEnumerator ShowIconTemporary()
        {
            dangerIcon.SetActive(true);
            yield return new WaitForSeconds(appearTime);
            dangerIcon.SetActive(false);
        }
    }

}