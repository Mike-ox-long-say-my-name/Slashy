using Core;
using UnityEngine;

namespace Miscellaneous
{
    public class CameraFollower : MonoBehaviour
    {
        [SerializeField] private float minBorderDistance = 15f;
        [SerializeField] private bool instantFirstMoves;
        [SerializeField] private Transform anchor;
        [SerializeField, Range(0, 1)] private float followSmoothTime = 0.3f;
        [SerializeField, Range(0, 1)] private float followSmoothTimeWhenBordered = 0.1f;

        [SerializeField] private Camera followingCamera;
        [SerializeField, HideInInspector] private bool hasCamera;
        [SerializeField, HideInInspector] private bool hasAnchor;

        private void Reset()
        {
            followingCamera = Camera.main;
        }

        private void OnValidate()
        {
            hasAnchor = anchor != null;
            hasCamera = followingCamera != null;
        }

        private float _followVelocity;

        private const int WarmFirstMoves = 5;
        private int _firstMovesRemained;
        
        private IBorderService _borderService;

        private void Follow(Transform target)
        {
            var targetX = target.position.x;
            var availableX = GetAvailableX(targetX);
            var smoothTime = ChooseSmoothTimeByBorderResult(availableX, targetX);

            var newX = Mathf.SmoothDamp(transform.position.x, availableX,
                ref _followVelocity, smoothTime);
            SetPositionX(newX, false);
        }

        private float ChooseSmoothTimeByBorderResult(float availableX, float targetX)
        {
            var isAdjustedByBorder = !Mathf.Approximately(availableX, targetX);
            var smoothTime = isAdjustedByBorder ? followSmoothTimeWhenBordered : followSmoothTime;
            return smoothTime;
        }

        private void SetPositionX(float x, bool checkForBorders = true)
        {
            var position = transform.position;
            var availableX = checkForBorders ? GetAvailableX(x) : x;
            transform.position = new Vector3(availableX, position.y, position.z);
        }

        private float GetAvailableX(float desiredX)
        {
            return _borderService.GetAvailableCameraX(followingCamera, desiredX, minBorderDistance);
        }

        private void Start()
        {
            _borderService = Container.Get<IBorderService>();
            
            if (instantFirstMoves)
            {
                _firstMovesRemained = WarmFirstMoves;
            }
            if (!hasCamera || !hasAnchor)
            {
                return;
            }
            
            SetPositionX(anchor.position.x);
        }

        private void LateUpdate()
        {
            if (!hasCamera || !hasAnchor)
            {
                return;
            }

            if (_firstMovesRemained > 0)
            {
                SetPositionX(anchor.position.x);
                _firstMovesRemained--;
            }
            else
            {
                Follow(anchor);
            }
        }
    }
}
