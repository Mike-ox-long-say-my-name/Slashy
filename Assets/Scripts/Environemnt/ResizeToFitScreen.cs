using System;
using UnityEngine;

namespace Environment
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class ResizeToFitScreen : MonoBehaviour
    {
        private SpriteRenderer _spriteRenderer;

        private void Awake()
        {
            if (Camera.main == null)
            {
                Debug.LogWarning("Camera not found", this);
                return;
            }

            _spriteRenderer = GetComponent<SpriteRenderer>();
            ResizeSpriteToScreen();
        }

        public void ResizeSpriteToScreen()
        {
            transform.localScale = new Vector3(1, 1, 1);

            var width = _spriteRenderer.sprite.bounds.size.x;
            var height = _spriteRenderer.sprite.bounds.size.y;

            var worldScreenHeight = Camera.main.orthographicSize * 2.0f;
            var worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;

            var scale = transform.localScale;
            scale.x = worldScreenWidth / width;
            scale.y = worldScreenHeight / height;
            transform.localScale = scale;
        }
    }
}