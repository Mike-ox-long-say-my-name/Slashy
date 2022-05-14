using Core.Player;
using Core.Player.Interfaces;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Levels
{
    public class LevelWarpManager : PublicSingleton<LevelWarpManager>
    {
        [SerializeField] private Image screenImage;
        [SerializeField] private float screenDarkeningTime = 1.8f;

        private void Start()
        {
            GameLoader.Instance.Exiting.AddListener(_ =>
            {
                StopAllCoroutines();
                SetScreenAlpha(0);
            });
        }

        public bool CanInitiateWarp { get; private set; } = true;

        private void SetScreenAlpha(float alpha)
        {
            var color = screenImage.color;
            color.a = alpha;
            screenImage.color = color;
        }

        public void InitiateWarp(Vector3 position, LevelWarpInfo info)
        {
            CanInitiateWarp = false;
            Warp(PlayerManager.Instance.PlayerInfo, position, info);
        }

        private void Warp(IPlayer playerInfo, Vector3 targetPosition, LevelWarpInfo info)
        {
            StartCoroutine(MoveAndLoadRoutine(playerInfo, targetPosition, info));
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

        private IEnumerator MoveAndLoadRoutine(IPlayer playerInfo, Vector3 target, LevelWarpInfo info)
        {
            playerInfo.IsFrozen = true;
            playerInfo.Hurtbox.Disable();
            yield return MoveRoutine(playerInfo, target);
            yield return LerpScreenAlphaRoutine(1, screenDarkeningTime);

            LoadNextLevel(playerInfo, info);
        }

        private IEnumerator LerpScreenAlphaRoutine(float targetAlpha, float time)
        {
            var difference = targetAlpha - screenImage.color.a;
            var passedTime = 0f;
            while (passedTime < time)
            {
                var deltaTime = Time.deltaTime;
                passedTime += deltaTime;
                SetScreenAlpha(screenImage.color.a + difference * Time.deltaTime);
                yield return null;
            }
        }

        private IEnumerator MoveAndReleaseRoutine(IPlayer playerInfo, Vector3 target)
        {
            yield return LerpScreenAlphaRoutine(0.4f, screenDarkeningTime);

            // Т.к. переход от 0.4 до 0 не заметен, но нам приходится ждать конца корутины
            SetScreenAlpha(0);

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
    }
}