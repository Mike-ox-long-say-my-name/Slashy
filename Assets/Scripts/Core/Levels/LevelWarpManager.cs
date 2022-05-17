using Core.Player;
using Core.Player.Interfaces;
using System.Collections;
using UnityEngine;

namespace Core.Levels
{
    public class LevelWarpManager : PublicSingleton<LevelWarpManager>
    {
        private LevelWarpInfo _info;

        public bool CanInitiateWarp { get; private set; } = true;

        public void InitiateWarp(Vector3 position, LevelWarpInfo info)
        {
            CanInitiateWarp = false;
            _info = info;
            PlayerManager.Instance.StartedWarping?.Invoke(position);
        }

        private void Warp(IPlayer playerInfo, LevelWarpInfo info)
        {
            StartCoroutine(MoveAndLoadRoutine(playerInfo, info));
        }

        private static IEnumerator MoveRoutine(IPlayer playerInfo, Vector3 target)
        {
            playerInfo.Animator.SetBool("is-walking", true);
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
            playerInfo.Animator.SetBool("is-walking", false);
        }

        private IEnumerator MoveAndLoadRoutine(IPlayer playerInfo, LevelWarpInfo info)
        {
            playerInfo.IsFrozen = true;
            playerInfo.Hurtbox.Disable();

            var time = BlackScreenManager.Instance.DefaultTime;
            BlackScreenManager.Instance.Blackout(time);
            yield return new WaitForSeconds(time);

            LoadNextLevel(playerInfo, info);
        }

        private IEnumerator MoveAndReleaseRoutine(IPlayer playerInfo, Vector3 target)
        {
            var time = BlackScreenManager.Instance.DefaultTime;
            BlackScreenManager.Instance.Whiteout(time);
            yield return new WaitForSeconds(time);

            yield return MoveRoutine(playerInfo, target);
            playerInfo.Hurtbox.Enable();
            playerInfo.IsFrozen = false;

            CanInitiateWarp = true;
        }

        private void LoadNextLevel(IPlayer playerInfo, LevelWarpInfo info)
        {
            PlayerManager.Instance.PlayerLoaded.AddListener(OnPlayerLoaded);

            var player = playerInfo.Player;
            _transferData = new PlayerTransferData
            {
                Health = player.Character.Health.Value,
                Balance = player.Character.Balance.Value,
                Stamina = player.Stamina.Value,
                StartPosition = info.StartPosition,
                StartTargetPosition = info.StartMoveTarget
            };

            GameLoader.Instance.LoadLevel(info.LevelName);
        }

        private struct PlayerTransferData
        {
            public float Health { get; set; }
            public float Stamina { get; set; }
            public float Balance { get; set; }
            public Vector3 StartPosition { get; set; }
            public Vector3 StartTargetPosition { get; set; }
        }

        private PlayerTransferData _transferData;

        private void OnPlayerLoaded()
        {
            PlayerManager.Instance.PlayerLoaded.RemoveListener(OnPlayerLoaded);

            var playerInfo = PlayerManager.Instance.PlayerInfo;
            playerInfo.IsFrozen = true;

            var player = playerInfo.Player;
            player.Character.Health.Value = _transferData.Health;
            player.Character.Balance.Value = _transferData.Balance;
            player.Stamina.Value = _transferData.Stamina;
            var movement = playerInfo.VelocityMovement.BaseMovement;
            movement.SetPosition(_transferData.StartPosition);

            StartCoroutine(MoveAndReleaseRoutine(playerInfo, _transferData.StartTargetPosition));
        }

        public void PlayerReady()
        {
            Warp(PlayerManager.Instance.PlayerInfo, _info);
        }
    }
}