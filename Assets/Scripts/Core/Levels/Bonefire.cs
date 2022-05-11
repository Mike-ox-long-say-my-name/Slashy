using JetBrains.Annotations;
using UnityEngine;

namespace Core.Levels
{
    public class Bonefire : MonoBehaviour
    {
        [SerializeField] private Vector3 respawnOffset;

        public Vector3 GetRespawnPosition()
        {
            return transform.position + respawnOffset;
        }

        [UsedImplicitly]
        public void TouchBonefire()
        {
            RespawnManager.Instance.UpdateRespawnData(this);
            print("Touched bonfire");
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawSphere(GetRespawnPosition(), 0.3f);
        }
    }
}