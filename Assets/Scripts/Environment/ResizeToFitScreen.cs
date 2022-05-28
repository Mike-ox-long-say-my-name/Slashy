using UnityEngine;

namespace Environment
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class ResizeToFitScreen : MonoBehaviour
    {
        private SpriteRenderer _spriteRenderer;
        private Camera _camera;
        private void Awake()
        {
            _camera = Camera.main;
            
            if (_camera == null)
            {
                Debug.LogWarning("Camera not found", this);
                return;
            }

            _spriteRenderer = GetComponent<SpriteRenderer>();
            ResizeSpriteToScreen();
            
        }

        private void ResizeSpriteToScreen()
        {
            transform.localScale = new Vector3(1, 1, 1);

            var sprite = _spriteRenderer.sprite;
            var width = sprite.bounds.size.x;
            var height = sprite.bounds.size.y;

            var worldScreenHeight = _camera.orthographicSize * 2.0f;
            
            var worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;

            var scale = transform.localScale;
            scale.x = worldScreenWidth / width;
            scale.y = worldScreenHeight / height;
            transform.localScale = scale;
        }
    }
}