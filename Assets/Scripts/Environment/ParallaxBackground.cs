using UnityEngine;

namespace Environment
{
    [RequireComponent(typeof(SpriteRenderer))]
    [DefaultExecutionOrder(10)]
    public class ParallaxBackground : MonoBehaviour
    {
        public float weight;

        private SpriteRenderer _spriteRenderer;

        private Transform _camera;
        private Vector3 _lastCameraPosition;

        private void Awake()
        {
            if (Camera.main == null)
            {
                Debug.LogWarning("Camera not found", this);
                enabled = false;
                return;
            }

            _spriteRenderer = GetComponent<SpriteRenderer>();
            _camera = Camera.main.transform;
            _lastCameraPosition = _camera.position;
        }

        private void LateUpdate()
        {
            var position = _camera.position;
            var delta = position - _lastCameraPosition;
            _spriteRenderer.transform.position += new Vector3(delta.x * weight, delta.y * weight, 0);

            _lastCameraPosition = position;
        }
    }
}