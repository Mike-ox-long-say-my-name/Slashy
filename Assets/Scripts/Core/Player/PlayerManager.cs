using Core.Player.Interfaces;
using UnityEngine;
using UnityEngine.Events;

namespace Core.Player
{
    public class PlayerManager
    {
        [SerializeField] private UnityEvent playerLoaded;
        [SerializeField] private UnityEvent playedDeadSequenceStarted;
        [SerializeField] private UnityEvent playedDeadSequenceEnded;
        [SerializeField] private UnityEvent playerTouchedBonfire;
        [SerializeField] private UnityEvent playedStartedAttacking;
        [SerializeField] private UnityEvent<Vector3> startedWarping;

        public UnityEvent PlayerLoaded => playerLoaded;
        public UnityEvent PlayedDeadSequenceStarted => playedDeadSequenceStarted;
        public UnityEvent PlayedDeadSequenceEnded => playedDeadSequenceEnded;
        public UnityEvent PlayerTouchedBonfire => playerTouchedBonfire;
        public UnityEvent PlayedStartedAttacking => playedStartedAttacking;
        public UnityEvent<Vector3> StartedWarping => startedWarping;

        public void ActivatePlayer()
        {
            TrySetActive(true);
        }

        public void DeactivatePlayer()
        {
            TrySetActive(false);
        }

        private void TrySetActive(bool value)
        {
            var player = PlayerInfo;
            if (player == null)
            {
                return;
            }

            var playerObject = player.PlayerObject;
            if (playerObject == null)
            {
                return;
            }

            playerObject.SetActive(value);
        }
    }
}