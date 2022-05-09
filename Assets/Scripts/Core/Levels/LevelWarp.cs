using System.Collections;
using Core.Player;
using Core.Player.Interfaces;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core.Levels
{
    [RequireComponent(typeof(BoxCollider))]
    public class LevelWarp : MonoBehaviour
    {
        [SerializeField] private SceneAsset levelWarp;
        [SerializeField] private Vector3 startPosition;
        [SerializeField] private Vector3 startTargetPosition;

        private void OnTriggerEnter(Collider other)
        {
            var player = other.GetComponent<IPlayer>();
            if (player != null)
            {
                Warp(player);
            }
        }

        private void Warp(IPlayer playerInfo)
        {
            var target = transform.position;
            target.y = 0;
            StartCoroutine(MoveAndLoadRoutine(playerInfo, target));
        }

        private static IEnumerator MoveRoutine(IPlayer playerInfo, Vector3 target)
        {
            var movement = playerInfo.VelocityMovement;
            var player = playerInfo.VelocityMovement.BaseMovement.Transform.position;

            while (true)
            {
                var targetDirection = target - player;
                targetDirection.y = 0;
                targetDirection.Normalize();

                movement.Move(targetDirection);

                var playerNewPosition = player;
                playerNewPosition.y = 0;

                const float margin = 0.2f;
                if (Vector3.Distance(target, playerNewPosition) < margin)
                {
                    break;
                }
                yield return null;
            }
        }

        private IEnumerator MoveAndLoadRoutine(IPlayer playerInfo, Vector3 target)
        {
            playerInfo.Hurtbox.Disable();
            yield return MoveRoutine(playerInfo, target);
            LoadNextLevel(playerInfo);
        }

        private static IEnumerator MoveAndReleaseRoutine(IPlayer playerInfo, Vector3 target)
        {
            yield return MoveRoutine(playerInfo, target);
            playerInfo.Hurtbox.Enable();
        }

        private void LoadNextLevel(IPlayer playerInfo)
        {
            SceneManager.sceneLoaded += OnSceneLoaded;

            var player = playerInfo.Player;
            _transferData = new PlayerTransferData
            {
                Health = player.Character.Health.Value,
                Balance = player.Character.Balance.Value,
                Stamina = player.Stamina.Value,
                StartPosition = startPosition,
                StartTargetPosition = startTargetPosition
            };

            SceneManager.LoadScene(levelWarp.name);
        }

        private struct PlayerTransferData
        {
            public float Health { get; set; }
            public float Stamina { get; set; }
            public float Balance { get; set; }
            public Vector3 StartPosition { get; set; }
            public Vector3 StartTargetPosition { get; set; }
        }

        private static PlayerTransferData _transferData;

        private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;

            var host = PlayerManager.Instance;
            var playerInfo = host.PlayerInfo;
            var player = playerInfo.Player;
            player.Character.Health.Value = _transferData.Health;
            player.Character.Balance.Value = _transferData.Balance;
            player.Stamina.Value = _transferData.Stamina;
            var movement = playerInfo.VelocityMovement.BaseMovement;
            movement.SetPosition(_transferData.StartPosition);

            host.StartCoroutine(MoveAndReleaseRoutine(playerInfo, _transferData.StartTargetPosition));
        }
    }
}