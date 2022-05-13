using Core.Player;
using Core.Player.Interfaces;
using JetBrains.Annotations;
using UnityEngine;

namespace Core.Levels
{
    public class Bonfire : MonoBehaviour, IInteractable
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
        
        private void TouchBonfire()
        {
            RespawnManager.Instance.UpdateRespawnData(this);
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawSphere(GetRespawnPosition(), 0.3f);
            Gizmos.DrawSphere(GetPlayerAnimationPosition(), 0.2f);
        }

        public bool IsInteractable { get; set; } = false;
        public InteractionMask Mask => InteractionMask.Bonfire;

        public InteractionResult Interact()
        {
            TouchBonfire();
            return new InteractionResult(InteractionType.TouchedBonfire, this);
        }
    }
}