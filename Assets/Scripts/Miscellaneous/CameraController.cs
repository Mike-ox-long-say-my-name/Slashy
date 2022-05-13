using Core;
using UnityEngine;

namespace Miscellaneous
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private float minBorderDistance = 15f;
        [SerializeField] private bool instantFirstMoves;
        [SerializeField] private Transform anchor;
        [SerializeField, Range(0, 1)] private float followSmoothTime = 0.3f;

        private float _followVelocity;

        // Спасибо классно работает
        private const int WarmFirstMoves = 5;
        private int _firstMovesRemained;

        private void Follow(Transform target)
        {
            var position = transform.position;
            var newX = Mathf.SmoothDamp(position.x, target.position.x,
                ref _followVelocity, followSmoothTime);
            SetPosition(newX);
        }

        private void SetPosition(float x)
        {
            var position = transform.position;
            var availableX = BorderManager.Instance.GetAvailableCameraX(position.x, x, minBorderDistance);
            transform.position = new Vector3(availableX, position.y, position.z);
        }

        private void Awake()
        {
            if (instantFirstMoves)
            {
                _firstMovesRemained = WarmFirstMoves;
            }
        }

        private void LateUpdate()
        {
            if (anchor == null)
            {
                return;
            }

            if (_firstMovesRemained > 0)
            {
                SetPosition(anchor.position.x);
                _firstMovesRemained--;
            }
            else
            {
                Follow(anchor);
            }
        }
    }
}
