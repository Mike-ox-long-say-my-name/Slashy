using System;
using Core.Player;
using Core.Player.Interfaces;
using System.Collections;
using UnityEditor;
using UnityEngine;

namespace Core.Levels
{
    [RequireComponent(typeof(BoxCollider))]
    public class LevelWarp : MonoBehaviour
    {
        [SerializeField] private SceneAsset levelWarp;
        [SerializeField] private Vector3 startPosition;
        [SerializeField] private Vector3 startTargetPosition;

        public void OnTriggered(Collider other)
        {
            if (other.GetComponent<IPlayer>() != null)
            {
                Warp(PlayerManager.Instance.PlayerInfo);
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
            var player = playerInfo.Transform;

            while (true)
            {
                var targetDirection = (target - player.position).WithZeroY();
                targetDirection.Normalize();

                movement.Move(targetDirection);

                var playerNewPosition = player.position.WithZeroY();

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
            playerInfo.IsFrozen = true;
            playerInfo.Hurtbox.Disable();
            yield return MoveRoutine(playerInfo, target);
            LoadNextLevel(playerInfo);
        }

        private static IEnumerator MoveAndReleaseRoutine(IPlayer playerInfo, Vector3 target)
        {
            yield return MoveRoutine(playerInfo, target);
            playerInfo.Hurtbox.Enable();
            playerInfo.IsFrozen = false;
        }

        private void LoadNextLevel(IPlayer playerInfo)
        {
            PlayerManager.Instance.PlayerLoaded.AddListener(OnPlayerLoaded);

            var player = playerInfo.Player;
            _transferData = new PlayerTransferData
            {
                Health = player.Character.Health.Value,
                Balance = player.Character.Balance.Value,
                Stamina = player.Stamina.Value,
                StartPosition = startPosition,
                StartTargetPosition = startTargetPosition
            };

            GameLoader.Instance.LoadLevel(levelWarp.name);
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

        private static void OnPlayerLoaded()
        {
            var host = PlayerManager.Instance;
            host.PlayerLoaded.RemoveListener(OnPlayerLoaded);

            var playerInfo = host.PlayerInfo;
            var player = playerInfo.Player;
            player.Character.Health.Value = _transferData.Health;
            player.Character.Balance.Value = _transferData.Balance;
            player.Stamina.Value = _transferData.Stamina;
            var movement = playerInfo.VelocityMovement.BaseMovement;
            movement.SetPosition(_transferData.StartPosition);

            host.StartCoroutine(MoveAndReleaseRoutine(playerInfo, _transferData.StartTargetPosition));
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawSphere(transform.position, 0.3f);
        }
    }
}