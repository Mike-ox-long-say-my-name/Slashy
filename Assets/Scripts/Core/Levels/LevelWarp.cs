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
        private ILevelWarper _levelWarper;

        private void Awake()
        {
            Construct();
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawSphere(GetTargetPosition(), 0.3f);
        }

        public void OnTriggered(Collider other)
        {
            if (ShouldInitiateWarp(other))
            {
                InitiateWarp();
            }
        }

        private void Construct()
        {
            _levelWarper = Container.Get<ILevelWarper>();
        }

        private void InitiateWarp()
        {
            _entered = true;
            
            var target = GetTargetPosition();
            var info = new LevelWarpInfo(levelWarp, startPosition, startTargetPosition);

            _levelWarper.InitiateWarp(target, info);
        }

        private bool ShouldInitiateWarp(Collider other)
        {
            return !_entered && _levelWarper.CanInitiateWarp && other.GetComponent<IPlayer>() != null;
        }

        private Vector3 GetTargetPosition()
        {
            return transform.position.WithZeroY();
        }
    }
}