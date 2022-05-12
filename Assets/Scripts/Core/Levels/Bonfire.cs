using Core.Player;
using Core.Player.Interfaces;
using JetBrains.Annotations;
using UnityEngine;

namespace Core.Levels
{
    public class Bonfire : MonoBehaviour
    {
        [SerializeField] private Vector3 respawnOffset;
        [SerializeField] private Vector3 playerPositionOffset = new Vector3(1, 0, 0);

        public Vector3 GetRespawnPosition()
        {
            return transform.position + respawnOffset;
        }

        public Vector3 GetPlayerAnimationPosition()
        {
            return transform.position + playerPositionOffset;
        }

        [UsedImplicitly]
        public void TouchBonfire()
        {
            RespawnManager.Instance.UpdateRespawnData(this);
            AlignPlayerForAnimation(PlayerManager.Instance.PlayerInfo);
        }

        private void AlignPlayerForAnimation(IPlayer player)
        {
            var bonfirePosition = transform.position;
            var animationPosition = GetPlayerAnimationPosition();
            var movement = player.VelocityMovement.BaseMovement;
            movement.SetPosition(animationPosition);
            movement.Rotate(bonfirePosition.x - animationPosition.x);
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawSphere(GetRespawnPosition(), 0.3f);
            Gizmos.DrawSphere(GetPlayerAnimationPosition(), 0.2f);
        }
    }
}