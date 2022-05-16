using Core.Player.Interfaces;
using UnityEngine;

namespace Core.Levels
{
    public class LevelWarp : MonoBehaviour
    {
        [SerializeField] private string levelWarp;
        [SerializeField] private Vector3 startPosition;
        [SerializeField] private Vector3 startTargetPosition;

        private bool _entered = false;

        public void OnTriggered(Collider other)
        {
            if (_entered || !LevelWarpManager.Instance.CanInitiateWarp || other.GetComponent<IPlayer>() == null)
            {
                return;
            }

            _entered = true;
            var target = GetTargetPosition();
            var info = new LevelWarpInfo(levelWarp, startPosition, startTargetPosition);
            LevelWarpManager.Instance.InitiateWarp(target, info);
        }

        private Vector3 GetTargetPosition()
        {
            return transform.position.WithZeroY();
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawSphere(GetTargetPosition(), 0.3f);
        }
    }
}